using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Groups;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}")]
public class GroupController(AccessTester tester,
    Lazy<GetGroupJob> getGroupJob) : CustomController
{
    [HttpGet]
    public IActionResult GetGroupDetails([FromRoute] Guid groupId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Validate()) return Forbid();

        getGroupJob.Value.Execute(new GetGroupJob.JobData(groupId));
        return HandleJobResult(getGroupJob.Value);
    }

    [HttpPatch]
    public IActionResult PatchGroup([FromRoute] Guid groupId, [FromBody] PatchGroup payload)
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public IActionResult DeleteGroup([FromRoute] Guid groupId)
    {
        throw new NotImplementedException();
    }
}