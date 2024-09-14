using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class TransferOwnershipJob(GroupService groupService): IJob
{
    public record JobData(Guid GroupId, Guid MemberId, Guid InvokerId);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        groupService.SelectGroup(data.GroupId, data.InvokerId);
        Error = groupService.TransferOwnership(data.MemberId);
        
        ApiResponse = groupService.GetFullApiModel();
    }
}