using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;

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
    public IActionResult AddGroup([FromBody] CreateGroup payload)
    {
        throw new NotImplementedException();
    }
}