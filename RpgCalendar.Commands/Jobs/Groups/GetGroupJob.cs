using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class GetGroupJob(RelationalDb db, ImageService imageServices, GroupService groupService): IJob
{
    public record JobData(Guid GroupId, Guid InvokerId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        ApiResponse = groupService.GetGroupInfo(data.GroupId, data.InvokerId).ToFullGroup();
    }
}