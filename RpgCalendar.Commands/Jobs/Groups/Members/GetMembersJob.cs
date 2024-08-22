using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class GetMembersJob(RelationalDb db): IJob
{
    public record JobData(Guid groupId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var members = db.GroupsMembers
            .Where(x => x.GroupId == data.groupId)
            .Include(x => x.User)
            .Select(x => new Member(x.User.Id, x.User.Nick, x.PermissionLevel));
        ApiResponse = new MembersList(members);
    }
}