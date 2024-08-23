using Microsoft.Extensions.Logging;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class PatchMembersLimitsJob(RelationalDb db, Logger<PatchMembersLimitsJob> logger): IJob
{
    public record JobData(Guid GroupId, uint Limit);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse => null;


    public void Execute(JobData data)
    {
        var group = db.Groups.FirstOrDefault(x => x.GroupId == data.GroupId);
        if (group is null)
        {
            Error = ErrorCode.GroupNotExists;
            return;
        }
        
        group.UserLimit = data.Limit;
        db.SaveChanges();
        logger.LogInformation("Group {GroupId} user limit changed to {Limit}.", data.GroupId, data.Limit);
    }
}
