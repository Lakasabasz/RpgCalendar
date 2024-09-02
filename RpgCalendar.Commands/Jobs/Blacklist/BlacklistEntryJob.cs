using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class BlacklistEntryJob: IJob
{
    public record JobData(Guid UserId, Guid BlacklistId);

    public ErrorCode? Error { get; }
    public IApiResponse? ApiResponse { get; }

    public void Execxute(JobData data)
    {
        throw new NotImplementedException();
    }
}