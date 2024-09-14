using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class CreateGroupJob(RelationalDb db, ImageService imageServices, GroupService groupService): IJob
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

        var user = db.Users.First(x => x.Id == data.owner);
        var userOwnedGroupsCount = db.Groups.Count(x => x.OwnerId == data.owner);
        if(userOwnedGroupsCount + 1 > user.GroupsLimit)
        {
            Error = ErrorCode.OwnedGroupsLimitReached;
            return;
        }
        
        Error = groupService.AddGroup(Guid.Empty, data.owner, data.groupName, data.profilePicture);
        
        ApiResponse = groupService.GetFullApiModel();
    }
}