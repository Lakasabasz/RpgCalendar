using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class RemoveMemberJob(RelationalDb db): IJob
{
    public record JobData(Guid GroupId, Guid MemberId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse => null;

    public void Execute(JobData data)
    {
        var group = db.Groups.First(x => x.GroupId == data.GroupId);
        if (group.OwnerId == data.MemberId)
        {
            Error = ErrorCode.CannotRemoveOwner;
            return;
        }

        var member = db.GroupsMembers
            .FirstOrDefault(x => x.GroupId == group.GroupId && x.UserId == data.MemberId);
        if (member is null)
        {
            Error = ErrorCode.MemberNotInGroup;
            return;
        }

        db.GroupsMembers.Remove(member);
        db.SaveChanges();
    }
}