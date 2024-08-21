using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Members;

public class GenerateInviteLinkJob(RelationalDb db): IJob
{
    public record JobData(Guid groupId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var invite = Invite.Prepare(data.groupId);
        db.GroupsInvites.Add(invite);
        db.SaveChanges();
        ApiResponse = new ExternalInvite(invite.InviteId);
    }
}