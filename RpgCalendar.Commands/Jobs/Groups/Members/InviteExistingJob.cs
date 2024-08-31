using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class InviteExistingJob(RelationalDb db, GroupService groupService): IJob
{
    public record JobData(Guid GroupId, string PrivateCode, Guid InvokerId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var user = db.Users.FirstOrDefault(x => x.PrivateCode == data.PrivateCode);
        if (user is null)
        {
            Error = ErrorCode.UserNotExists;
            return;
        }

        Error = groupService.AddMember(data.GroupId, user.Id);

        var groupInfo = groupService.GetGroupInfo(data.GroupId, data.InvokerId);
        
        var members = db.GroupsMembers
            .Where(x => x.GroupId == data.GroupId)
            .Include(x => x.User)
            .Select(x => new Member(x.User.Id, x.User.Nick, x.PermissionLevel));
        ApiResponse = new MembersList(members, groupInfo.group.UserLimit);
    }
}