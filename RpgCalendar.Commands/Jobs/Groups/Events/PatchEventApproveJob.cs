using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class PatchEventApproveJob(EventService service): IJob
{
    public record JobData(Guid InvokerId, Guid EventId);
    
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        service.SelectEvent(data.EventId);
        
        Error = service.Approve(data.InvokerId);
        
        ApiResponse = service.GetFullApiModel();
    }
}