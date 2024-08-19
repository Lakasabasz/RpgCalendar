using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}/members")]
public class MemberController : Controller
{
    [HttpGet]
    public IActionResult GetMemberList()
    {
        throw new NotImplementedException();
    }

    [HttpPost("invite")]
    public IActionResult InviteMember()
    {
        throw new NotImplementedException();
    }

    [HttpPost("invite/external")]
    public IActionResult InviteExternal()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{memberId:guid}")]
    public IActionResult RemoveMember()
    {
        throw new NotImplementedException();
    }
}