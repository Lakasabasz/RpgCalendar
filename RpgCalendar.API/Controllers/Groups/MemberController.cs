using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}/members")]
public class MemberController : Controller
{
    [HttpGet]
    public IActionResult GetMemberList([FromRoute] Guid groupId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("invite")]
    public IActionResult InviteMember([FromRoute] Guid groupId, [FromBody] InviteMember payload)
    {
        throw new NotImplementedException();
    }

    [HttpPost("invite/external")]
    public IActionResult InviteExternal([FromRoute] Guid groupId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{memberId:guid}")]
    public IActionResult RemoveMember([FromRoute] Guid groupId, [FromRoute] Guid memberId)
    {
        throw new NotImplementedException();
    }
}