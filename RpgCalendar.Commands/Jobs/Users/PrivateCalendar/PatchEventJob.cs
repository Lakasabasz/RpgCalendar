using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class PatchEventJob(RelationalDb db): IJob
{
    public record JobData(
        Guid EventId,
        string? Title, string? Description,
        DateOnly? StartingDay, TimeOnly? StartingHour,
        DateOnly? EndingDay, TimeOnly? EndingHour,
        bool? IsOnline, string? Location);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataQuery
    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
    public void Execute(JobData data)
    {
        var @event = db.PrivateEvents.First(x => x.EventId == data.EventId);
        if (@event.Title is null != data.Title is null || @event.Description is null != data.Description is null)
        {
            Error = ErrorCode.CannotChangeEventType;
            return;
        }
        DateTime patchedStarting = new DateTime(data.StartingDay ?? DateOnly.FromDateTime(@event.StartTime),
            data.StartingHour ?? TimeOnly.FromDateTime(@event.StartTime));
        DateTime patchedEnding = new DateTime(data.EndingDay ?? DateOnly.FromDateTime(@event.EndTime),
            data.EndingHour ?? TimeOnly.FromDateTime(@event.EndTime));
        if (patchedStarting > patchedEnding)
        {
            Error = ErrorCode.PatchInvalidTimeRange;
            return;
        }

        @event.Title = data.Title ?? @event.Title;
        @event.Description = data.Description ?? @event.Description;
        @event.StartTime = patchedStarting;
        @event.EndTime = patchedEnding;
        @event.Location = data.Location ?? @event.Location;
        @event.IsOnline = data.IsOnline ?? @event.IsOnline;

        db.SaveChanges();
        var saved = db.PrivateEvents.First(x => x.EventId == data.EventId);
        ApiResponse = FullPrivateEvent.FromDateTime(saved.EventId, saved.OwnerId,
            saved.Title, saved.Description, saved.StartTime, saved.EndTime, saved.IsOnline, saved.Location);
    }

}