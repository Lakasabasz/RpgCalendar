using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class BlacklistEntryJob(RelationalDb db): IJob
{
    public record JobData(Guid InvokerId, Guid BlacklistId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; }

    public void Execute(JobData data)
    {
        var user = db.Users.FirstOrDefault(x => x.Id == data.BlacklistId);
        if(user is not null)
        {
            if(db.BlacklistUsers.FirstOrDefault(x => x.EntryOwnerId == data.InvokerId && x.BlacklistedUserId == user.Id) is not null)
            {
                Error = ErrorCode.UserAlreadyBlacklisted;
                return;
            }
        }
    }
}