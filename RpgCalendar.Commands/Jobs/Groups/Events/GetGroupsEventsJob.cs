using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class GetGroupsEventsJob(GroupService service): IJob
{
    public record JobData(DateTime From, DateTime To, Guid GroupId, Guid InvokerId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        service.SelectGroup(data.GroupId, data.InvokerId);

        ApiResponse = service.GetEventsListApiModel(data.From, data.To);
    }

}