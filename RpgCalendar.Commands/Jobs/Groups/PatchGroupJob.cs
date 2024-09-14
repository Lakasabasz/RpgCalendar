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
        groupService.SelectGroup(data.GroupId, data.InvokerId);
        if(data.ImageId is not null)
        {
            Error = groupService.UpdateImage(data.ImageId.Value, false);
            if (Error is not null) return;
        }
        
        if(data.Name is not null)
        {
            Error = groupService.UpdateName(data.Name, false);
            if (Error is not null) return;
        }
        
        db.SaveChanges();
        ApiResponse = groupService.SelectGroup(data.GroupId, data.InvokerId).GetFullApiModel();
    }
}