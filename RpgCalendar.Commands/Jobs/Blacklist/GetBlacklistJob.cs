using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class GetBlacklistJob: IJob
{
    public record JobData(Guid UserId);
    public ErrorCode? Error { get; }
    public IApiResponse? ApiResponse { get; }

    public void Execute(JobData data)
    {
        throw new NotImplementedException();
    }
}