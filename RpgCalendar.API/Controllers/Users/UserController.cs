using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Users;

namespace RpgCalendar.API.Controllers.Users;

[Authorize]
[ApiController, Route("/users/{userId:guid}")]
public class UserController(AccessTester tester,
    Lazy<PatchOwnedGroupLimitJob> patchOwnedGroupLimit) : CustomController
{
    [HttpPatch("limits")]
    public IActionResult UpdateLimits([FromRoute] Guid userId, [FromBody] PatchUserOwnedGroupLimits limit)
    {
        if (!Privileged) Forbid();
        
        patchOwnedGroupLimit.Value.Execute(new PatchOwnedGroupLimitJob.JobData(userId, limit.Limit));
        return HandleJobResult(patchOwnedGroupLimit.Value);
    }
}