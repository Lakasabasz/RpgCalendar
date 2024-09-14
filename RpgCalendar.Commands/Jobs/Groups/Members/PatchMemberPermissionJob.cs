using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class PatchMemberPermissionJob(RelationalDb db): IJob
{
    public record JobData(Guid GroupId, Guid MemberId, PermissionLevel Permission);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var member = db.GroupsMembers.FirstOrDefault(x => x.GroupId == data.GroupId && x.UserId == data.MemberId);
        if(member is null)
        {
            Error = ErrorCode.MemberNotInGroup;
            return;
        }
        if(member.PermissionLevel == PermissionLevel.Owner)
        {
            Error = ErrorCode.CannotChangeOwnerPermission;
            return;
        }

        member.PermissionLevel = data.Permission;
        db.SaveChanges();
        var group = db.Groups.First(x => x.GroupId == data.GroupId);
        var members = db.GroupsMembers
            .Where(x => x.GroupId == data.GroupId)
            .Include(x => x.User)
            .Select(x => new MemberApiModel(x.UserId, x.User.Nick, x.PermissionLevel));
        ApiResponse = new MembersListApiModel(members, group.UserLimit);
    }
}