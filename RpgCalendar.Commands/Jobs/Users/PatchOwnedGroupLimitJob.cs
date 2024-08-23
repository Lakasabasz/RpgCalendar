using Microsoft.Extensions.Logging;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users;

public class PatchOwnedGroupLimitJob(RelationalDb db, Logger<PatchOwnedGroupLimitJob> logger): IJob
{
    public record JobData(Guid UserId, uint Limit);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse => null;
    public void Execute(JobData data)
    {
        var user = db.Users.FirstOrDefault(x => x.Id == data.UserId);
        if (user is null)
        {
            Error = ErrorCode.UserNotExists;
            return;
        }
        
        user.GroupsLimit = data.Limit;
        db.SaveChanges();
        logger.LogInformation("Group {GroupId} user limit changed to {Limit}.", data.UserId, data.Limit);
    }
}
