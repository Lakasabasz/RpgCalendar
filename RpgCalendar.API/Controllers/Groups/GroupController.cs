using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}")]
public class GroupController : Controller
{
    [HttpGet]
    public IActionResult GetGroupDetails()
    {
        throw new NotImplementedException();
    }

    [HttpPatch]
    public IActionResult PatchGroup()
    {
        throw new NotImplementedException();
    }

    [HttpDelete]
    public IActionResult DeleteGroup()
    {
        throw new NotImplementedException();
    }
}