using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class DeleteEventJob(PrivateEventService service): IJob
{
    public record JobData(Guid eventId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse => null;

    public void Execute(JobData data)
    {
        service.SelectEvent(data.eventId);
        service.Delete();
    }
}