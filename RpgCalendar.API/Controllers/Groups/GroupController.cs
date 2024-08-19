using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}")]
public class GroupController : Controller
{
    [HttpGet]
    public IActionResult GetGroupDetails([FromRoute] Guid groupId)
    {
        throw new NotImplementedException();
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