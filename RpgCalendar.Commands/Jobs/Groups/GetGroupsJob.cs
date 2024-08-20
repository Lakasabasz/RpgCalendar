using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class GetGroupsJob(RelationalDb db, ImageService imageService): IJob
{
    public record JobData(Guid userId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var groups = db.GroupsMembers.Include(x => x.Group)
            .Where(x => x.UserId == data.userId)
            .Select(x => new GroupShort(x.Group.GroupId, x.Group.Name,
                imageService.GetImageUrl(x.Group.ProfilePicture), x.Group.CreationDate));
        ApiResponse = new GroupsList(groups);
    }
}