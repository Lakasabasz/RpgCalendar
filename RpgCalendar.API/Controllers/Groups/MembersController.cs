using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Tools;
using RpgCalendar.Commands.Jobs.Groups.Members;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}/members")]
public class MembersController(AccessTester tester,
    Lazy<GetMembersJob> getMembersJob,
    Lazy<InviteExistingJob> inviteExistingJob,
    Lazy<GenerateInviteLinkJob> generateInviteLinkJob,
    Lazy<RemoveMemberJob> removeMemberJob) : CustomController
{
    [HttpGet]
    public IActionResult GetMemberList([FromRoute] Guid groupId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Validate()) return Forbid();

        getMembersJob.Value.Execute(new GetMembersJob.JobData(groupId));
        return HandleJobResult(getMembersJob.Value);
    }

    [HttpPost("invite")]
    public IActionResult InviteMember([FromRoute] Guid groupId, [FromBody] InviteMember payload)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();

        inviteExistingJob.Value.Execute(new InviteExistingJob.JobData(groupId, payload.PrivateCode, Invoker.Id));
        return HandleJobResult(inviteExistingJob.Value);
    }

    [HttpPost("invite/external")]
    public IActionResult InviteExternal([FromRoute] Guid groupId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();

        generateInviteLinkJob.Value.Execute(new GenerateInviteLinkJob.JobData(groupId));
        return HandleJobResult(generateInviteLinkJob.Value);
    }

    [HttpDelete("{memberId:guid}")]
    public IActionResult RemoveMember([FromRoute] Guid groupId, [FromRoute] Guid memberId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();

        removeMemberJob.Value.Execute(new RemoveMemberJob.JobData(groupId, memberId));
        return HandleJobResult(removeMemberJob.Value);
    }
}