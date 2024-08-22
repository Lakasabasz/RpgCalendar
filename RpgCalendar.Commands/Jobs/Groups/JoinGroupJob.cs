using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class JoinGroupJob(RelationalDb db, ImageService imageService): IJob
{
    public record JobData(Guid InvokerId, Guid InviteId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var invite = db.GroupsInvites.FirstOrDefault(x => x.InviteId == data.InviteId);
        if (invite is null)
        {
            Error = ErrorCode.InviteNotExists;
            return;
        }

        if (db.GroupsMembers.FirstOrDefault(x => x.GroupId == invite.GroupId && x.UserId == data.InvokerId) is not null)
        {
            Error = ErrorCode.UserAlreadyRegistered;
            return;
        }

        var group = db.Groups.First(x => x.GroupId == invite.GroupId);
        db.GroupsMembers.Add(GroupMembers.Prepare(data.InviteId, invite.GroupId, PermissionLevel.Member));
        db.GroupsInvites.Remove(invite);
        db.SaveChanges();
        ApiResponse = new GroupFull(group.GroupId, group.OwnerId, group.Name,
            imageService.GetImageUrl(group.ProfilePicture), group.CreationDate);
    }
}