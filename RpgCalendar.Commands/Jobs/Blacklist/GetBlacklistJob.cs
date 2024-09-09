using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class GetBlacklistJob(RelationalDb db): IJob
{
    public record JobData(Guid UserId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        ApiResponse = BlacklistTools.PrepareBlacklistResponse(db, data.UserId);
    }
}