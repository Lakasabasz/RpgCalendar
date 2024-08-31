using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class PatchGroupJob(RelationalDb db, ImageService imageService, GroupService groupService): IJob
{
    public record JobData(Guid GroupId, Guid InvokerId, string? Name, Guid? ImageId);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        if(data.ImageId is not null)
        {
            Error = groupService.UpdateImage(out _, data.GroupId, data.ImageId.Value, data.InvokerId, false);
            if (Error is not null) return;
        }
        
        if(data.Name is not null)
        {
            Error = groupService.UpdateName(out _, data.GroupId, data.Name, data.InvokerId, false);
            if (Error is not null) return;
        }
        
        db.SaveChanges();
        ApiResponse = groupService.GetGroupInfo(data.GroupId, data.InvokerId).ToFullGroup();
    }
}