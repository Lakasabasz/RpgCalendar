using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class BlacklistEntryJob(RelationalDb db): IJob
{
    public record JobData(Guid InvokerId, Guid BlacklistId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var user = db.Users.FirstOrDefault(x => x.Id == data.BlacklistId);
        if(user is not null) 
        {
            BlacklistUser(data.InvokerId, data.BlacklistId);
            return;
        }

        var group = db.Groups.FirstOrDefault(x => x.GroupId == data.BlacklistId);
        if(group is not null)
        {
            BlacklistGroup(data.InvokerId, data.BlacklistId);
            return;
        }

        Error = ErrorCode.BlacklistIdInvalid;
    }
    
    private void PrepareBlacklistResponse(Guid invokerId)
    {
        var users = db.BlacklistUsers.Where(x => x.EntryOwnerId == invokerId)
            .Include(x => x.BlacklistedUser)
            .Select(x => new UserShortModel(x.BlacklistedUser.Id, x.BlacklistedUser.Nick));
        var groups = db.BlacklistGroups.Where(x => x.EntryOwnerId == invokerId)
            .Include(x => x.BlacklistedGroup)
            .Select(x => new GroupShort(x.BlacklistedGroup.GroupId, x.BlacklistedGroup.Name));

        ApiResponse = new BlacklistModel(users, groups);
    }

    private void BlacklistGroup(Guid invokerId, Guid groupId)
    {
        if(db.BlacklistGroups.FirstOrDefault(x => x.EntryOwnerId == invokerId && x.BlacklistedGroupId == groupId) is not null)
        {
            Error = ErrorCode.GroupAlreadyBlacklisted;
            return;
        }
        
        if(db.GroupsMembers.FirstOrDefault(x => x.UserId == invokerId && x.GroupId == groupId) is not null)
        {
            Error = ErrorCode.CannotBlacklistOwnGroup;
            return;
        }
        
        db.BlacklistGroups.Add(Database.Models.BlacklistGroup.Prepare(invokerId, groupId));
        db.SaveChanges();
        PrepareBlacklistResponse(invokerId);
    }

    private void BlacklistUser(Guid invokerId, Guid userId)
    {
        if(db.BlacklistUsers.FirstOrDefault(x => x.EntryOwnerId == invokerId && x.BlacklistedUserId == userId) is not null)
        {
            Error = ErrorCode.UserAlreadyBlacklisted;
            return;
        }
        db.BlacklistUsers.Add(Database.Models.BlacklistUser.Prepare(invokerId, userId));
        db.SaveChanges();
        
        PrepareBlacklistResponse(invokerId);
    }
}