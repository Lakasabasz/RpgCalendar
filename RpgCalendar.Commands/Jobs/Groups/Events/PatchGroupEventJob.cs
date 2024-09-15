using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class PatchGroupEventJob(RelationalDb db, EventService service): IJob
{
    public record JobData(Guid EventId, string? Title, string? Description, string? Location, bool? IsOnline, string? Summary,
        DateTime? Start, DateTime? End);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        service.SelectEvent(data.EventId);
        if(data.Title is not null)
        {
            Error = service.UpdateTitle(data.Title, false);
            if (Error is not null) return;
        }
        if(data.Description is not null)
        {
            Error = service.UpdateDescription(data.Description, false);
            if (Error is not null) return;
        }
        if(data.Location is not null)
        {
            Error = service.UpdateLocation(data.Location, false);
            if (Error is not null) return;
        }
        if(data.IsOnline is not null)
        {
            Error = service.UpdateOnline(data.IsOnline, false);
            if (Error is not null) return;
        }
        if(data.Summary is not null)
        {
            Error = service.UpdateSummary(data.Summary, false);
            if (Error is not null) return;
        }
        if(data.Start is not null || data.End is not null)
        {
            Error = service.UpdateData(data.Start, data.End, false);
            if (Error is not null) return;
        }

        db.SaveChanges();
        ApiResponse = service.GetFullApiModel();
    }
}