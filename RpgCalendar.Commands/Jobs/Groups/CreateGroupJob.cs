using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class CreateGroupJob(RelationalDb db, ImageService imageServices): IJob
{
    public record JobData(Guid owner, string groupName, Guid? profilePicture); 
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        if (data.profilePicture is not null && !imageServices.Contains(data.profilePicture.Value))
        {
            Error = ErrorCode.ImageNotExists;
            return;
        }

        var group = Group.Prepare(data.owner, data.groupName, data.profilePicture);
        db.Groups.Add(group);
        db.SaveChanges();
        var dbGroup = db.Groups.First(x => group.GroupId == x.GroupId);
        ApiResponse = new GroupFull(dbGroup.GroupId, dbGroup.OwnerId, dbGroup.Name, 
            imageServices.GetImageUrl(dbGroup.ProfilePicture), dbGroup.CreationDate);
    }
}