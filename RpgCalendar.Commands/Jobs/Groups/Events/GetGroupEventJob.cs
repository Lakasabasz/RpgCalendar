using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class GetGroupEventJob(EventService service): IJob
{
    public record JobData(Guid EventId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        ApiResponse = service.SelectEvent(data.EventId).GetFullApiModel();
    }
}