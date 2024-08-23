using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class InviteExistingJob(RelationalDb db): IJob
{
    public record JobData(Guid GroupId, string PrivateCode);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var user = db.Users.FirstOrDefault(x => x.PrivateCode == data.PrivateCode);
        if (user is null)
        {
            Error = ErrorCode.UserNotExists;
            return;
        }

        if (db.GroupsMembers.FirstOrDefault(x => x.UserId == user.Id) is not null)
        {
            Error = ErrorCode.UserAlreadyInGroup;
            return;
        }
        
        var group = db.Groups.First(x => x.GroupId == data.GroupId);
        if(db.GroupsMembers.Count(x => x.GroupId == data.GroupId) + 1 > group.UserLimit)
        {
            Error = ErrorCode.MembersLimitReached;
            return;
        }

        db.GroupsMembers.Add(GroupMembers.Prepare(user.Id, data.GroupId, PermissionLevel.Member));
        db.SaveChanges();
        var members = db.GroupsMembers
            .Where(x => x.GroupId == data.GroupId)
            .Include(x => x.User)
            .Select(x => new Member(x.User.Id, x.User.Nick, x.PermissionLevel));
        ApiResponse = new MembersList(members, group.UserLimit);
    }
}