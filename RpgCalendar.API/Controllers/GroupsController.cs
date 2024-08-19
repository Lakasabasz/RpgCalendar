using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RpgCalendar.API.Controllers;

[Authorize]
[ApiController, Route("/groups")]
public class GroupsController : CustomController
{
    [HttpGet]
    public IActionResult GetUserGroups()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult AddGroup()
    {
        throw new NotImplementedException();
    }
}