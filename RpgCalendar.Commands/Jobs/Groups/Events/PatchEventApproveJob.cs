using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class PatchEventApproveJob(GroupEventService service): IJob
{
    public record JobData(Guid InvokerId, Guid EventId);
    
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        service.SelectEvent(data.EventId);
        
        Error = service.SetRelation(data.InvokerId, RelationTowardsEventEnum.HardAccept);
        
        ApiResponse = service.GetFullApiModel();
    }
}