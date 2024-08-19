using Microsoft.Extensions.Logging;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;
using PrivateEvent = RpgCalendar.Database.Models.PrivateEvent;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class PatchEventJob(RelationalDb db): IJob
{
    public record JobData(
        Guid EventId,
        string? Title, string? Description,
        DateOnly? StartingDay, TimeOnly? StartingHour,
        DateOnly? EndingDay, TimeOnly? EndingHour);
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
        DateTime patchedStarting = new DateTime(data.StartingDay ?? DateOnly.FromDateTime(@event.Start),
            data.StartingHour ?? TimeOnly.FromDateTime(@event.Start));
        DateTime patchedEnding = new DateTime(data.EndingDay ?? DateOnly.FromDateTime(@event.End),
            data.EndingHour ?? TimeOnly.FromDateTime(@event.End));
        if (patchedStarting > patchedEnding)
        {
            Error = ErrorCode.PatchInvalidTimeRange;
            return;
        }

        @event.Title = data.Title;
        @event.Description = data.Description;
        @event.Start = patchedStarting;
        @event.End = patchedEnding;

        db.SaveChanges();
        var saved = db.PrivateEvents.First(x => x.EventId == data.EventId);
        ApiResponse = FullPrivateEvent.FromDateTime(saved.EventId, saved.OwnerId,
            saved.Title, saved.Description, saved.Start, saved.End);
    }

}