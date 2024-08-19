using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class DeleteEventJob(RelationalDb db): IJob
{
    public record JobData(Guid eventId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse => null;

    public void Execute(JobData data)
    {
        var @event = db.PrivateEvents.First(x => x.EventId == data.eventId);
        db.PrivateEvents.Remove(@event);
        db.SaveChanges();
    }
}