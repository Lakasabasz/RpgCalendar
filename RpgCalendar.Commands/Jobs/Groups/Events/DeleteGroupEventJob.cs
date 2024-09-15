using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class DeleteGroupEventJob(EventService service): IJob
{
    public record JobData(Guid EventId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse => null;

    public void Execute(JobData data)
    {
        service.SelectEvent(data.EventId).Delete();
    }
}