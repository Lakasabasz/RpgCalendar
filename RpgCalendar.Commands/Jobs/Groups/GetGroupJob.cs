using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class GetGroupJob(GroupService groupService): IJob
{
    public record JobData(Guid GroupId, Guid InvokerId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        ApiResponse = groupService.SelectGroup(data.GroupId, data.InvokerId).GetFullApiModel();
    }
}