using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}/calendar")]
public class CalendarController: CustomController
{
    [HttpGet]
    public IActionResult EventLists([FromRoute] Guid groupId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public IActionResult AddEvent([FromRoute] Guid groupId)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{eventId:guid}")]
    public IActionResult EventDetails([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{eventId:guid}")]
    public IActionResult DeleteEvent([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{eventId:guid}")]
    public IActionResult EditEvent([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        throw new NotImplementedException();
    }
}