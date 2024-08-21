using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class PatchGroupJob(RelationalDb db, ImageService imageService): IJob
{
    public record JobData(Guid GroupId, string? Name, Guid? ImageId);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        if (data.ImageId is not null && imageService.Contains(data.ImageId.Value))
        {
            Error = ErrorCode.ImageNotExists;
            return;
        }

        var group = db.Groups.First(x => x.GroupId == data.GroupId);
        group.Name = data.Name ?? group.Name;
        group.ProfilePicture = data.ImageId ?? group.ProfilePicture;
        db.SaveChanges();
        var savedGroup = db.Groups.First(x => x.GroupId == data.GroupId);
        ApiResponse = new GroupFull(savedGroup.GroupId, savedGroup.OwnerId, savedGroup.Name,
            imageService.GetImageUrl(savedGroup.ProfilePicture), savedGroup.CreationDate);
    }
}