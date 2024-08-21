using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class DeleteGroupJob(RelationalDb db): IJob
{
    public record JobData(Guid groupId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse => null;
    public void Execute(JobData data)
    {
        var members = db.GroupsMembers.Where(x => x.GroupId == data.groupId);
        var invites = db.GroupsInvites.Where(x => x.GroupId == data.groupId);
        var group = db.Groups.First(x => x.GroupId == data.groupId);
        db.GroupsMembers.RemoveRange(members);
        db.GroupsInvites.RemoveRange(invites);
        db.Groups.RemoveRange(group);
        db.SaveChanges();
    }
}