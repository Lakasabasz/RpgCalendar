using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class PatchEventJob(RelationalDb db, PrivateEventService service): IJob
{
    public record JobData(
        Guid EventId,
        string? Title, string? Description,
        DateOnly? StartingDay, TimeOnly? StartingHour,
        DateOnly? EndingDay, TimeOnly? EndingHour,
        bool? IsOnline, string? Location,
        bool OverwriteApprovals);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataQuery
    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
    public void Execute(JobData data)
    {
        service.SelectEvent(data.EventId);
        if(data.Title is not null)
        {
            Error = service.UpdateTitle(data.Title, false);
            if(Error is not null) return;
        }
        
        if(data.Description is not null)
        {
            Error = service.UpdateDescription(data.Description, false);
            if(Error is not null) return;
        }
        
        if(data.StartingDay is not null || data.StartingHour is not null || data.EndingDay is not null || data.EndingHour is not null)
        {
            DateTime patchedStarting = new DateTime(data.StartingDay ?? DateOnly.FromDateTime(service.Event.StartTime),
                data.StartingHour ?? TimeOnly.FromDateTime(service.Event.StartTime));
            DateTime patchedEnding = new DateTime(data.EndingDay ?? DateOnly.FromDateTime(service.Event.EndTime),
                data.EndingHour ?? TimeOnly.FromDateTime(service.Event.EndTime));
            Error = service.UpdateDateTime(patchedStarting, patchedEnding, data.OverwriteApprovals);
        }
        
        if(data.Location is not null)
        {
            Error = service.UpdateLocation(data.Location, false);
            if(Error is not null) return;
        }
        
        if(data.IsOnline is not null)
        {
            Error = service.UpdateIsOnline(data.IsOnline, false);
            if(Error is not null) return;
        }

        db.SaveChanges();

        ApiResponse = service.GetFullApiModel();
    }
}