using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Commands.DTOs;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class GetEventsJob(RelationalDb db): IJob
{
    public record JobData(Guid userId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(TimePagination pagination, JobData data)
    {
        var events = db.PrivateEvents
            .Where(x => x.OwnerId == data.userId)
            .Where(x => !x.SimpleAbsence)
            .Where(x => (pagination.Start <= x.StartTime && x.StartTime <= pagination.End)
                        || (pagination.Start <= x.EndTime && x.EndTime <= pagination.End))
            .Select(x => PrivateEvent.FromDateTime(x.EventId, x.Title ?? "", x.Description ?? "",
                x.StartTime, x.EndTime, x.IsOnline, x.Location));
        ApiResponse = new PrivateEvents(events);
    }
}