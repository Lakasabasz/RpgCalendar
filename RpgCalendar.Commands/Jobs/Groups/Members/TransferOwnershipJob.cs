using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class TransferOwnershipJob(RelationalDb db, ImageService imageService): IJob
{
    public record JobData(Guid GroupId, Guid MemberId);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var membership = db.GroupsMembers
            .FirstOrDefault(x => x.GroupId == data.GroupId && x.UserId == data.MemberId);
        if(membership is null)
        {
            Error = ErrorCode.MemberNotInGroup;
            return;
        }
        
        var newOwner = db.Users.First(x => x.Id == data.MemberId);
        var newOwnerCurrentGroups = db.Groups.Count(x => x.OwnerId == data.MemberId);
        
        if(newOwnerCurrentGroups + 1 > newOwner.GroupsLimit)
        {
            Error = ErrorCode.OwnedGroupsLimitReached;
            return;
        }

        membership.PermissionLevel = PermissionLevel.Owner;
        var group = db.Groups.First(x => x.GroupId == data.GroupId);
        var ownerMembership = db.GroupsMembers
            .First(x => x.GroupId == data.GroupId && x.UserId == group.OwnerId);
        group.OwnerId = data.MemberId;
        ownerMembership.GroupId = data.GroupId;
        db.SaveChanges();
        var saved = db.Groups.First(x => x.GroupId == data.GroupId);
        ApiResponse = new GroupFull(saved.GroupId, saved.OwnerId, saved.Name, 
            imageService.GetImageUrl(saved.ProfilePicture), saved.CreationDate);
    }
}