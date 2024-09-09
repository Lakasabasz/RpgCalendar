using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class RemoveBlacklistEntryJob(RelationalDb db): IJob
{
    public record JobData(Guid UserId, Guid BlacklistId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var blacklistedGroup = db.BlacklistGroups
            .FirstOrDefault(x => x.EntryOwnerId == data.UserId && x.BlacklistedGroupId == data.BlacklistId);
        if(blacklistedGroup is not null)
        {
            db.BlacklistGroups.Remove(blacklistedGroup);
            db.SaveChanges();
            ApiResponse = BlacklistTools.PrepareBlacklistResponse(db, data.UserId);
            return;
        }
        
        var blacklistedUser = db.BlacklistUsers
            .FirstOrDefault(x => x.EntryOwnerId == data.UserId && x.BlacklistedUserId == data.BlacklistId);
        if(blacklistedUser is not null)
        {
            db.BlacklistUsers.Remove(blacklistedUser);
            db.SaveChanges();
            ApiResponse = BlacklistTools.PrepareBlacklistResponse(db, data.UserId);
            return;
        }

        Error = ErrorCode.BlacklistIdInvalid;
    }
}