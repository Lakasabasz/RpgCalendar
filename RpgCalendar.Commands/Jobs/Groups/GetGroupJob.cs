using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class GetGroupJob(RelationalDb db, ImageService imageServices): IJob
{
    public record JobData(Guid GroupId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var dbGroup = db.Groups.First(x => data.GroupId == x.GroupId);
        ApiResponse = new GroupFull(dbGroup.GroupId, dbGroup.OwnerId, dbGroup.Name, 
            imageServices.GetImageUrl(dbGroup.ProfilePicture), dbGroup.CreationDate);
    }
}