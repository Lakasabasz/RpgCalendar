using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.Commands;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Users;

[Authorize]
[ApiController, Route("/users/{userId:guid}/calendar")]
public class CalendarController(AccessTester tester) : CustomController
{
    [HttpGet("absences")]
    public IActionResult GetAbsences([FromRoute] Guid userId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId)) Forbid();
        return Ok();
    }

    [HttpPost("absences")]
    public IActionResult AddAbsence()
    {
        return Ok();
    }

    [HttpDelete("absences/{absenceId:guid}")]
    public IActionResult DeleteAbsence()
    {
        return Ok();
    }
    
    [HttpGet("events")]
    public IActionResult GetEvents()
    {
        return Ok();
    }

    [HttpPost("events")]
    public IActionResult AddEvent()
    {
        return Ok();
    }
    
    [HttpPatch("events/{eventId:guid}")]
    public IActionResult UpdateEvent()
    {
        return Ok();
    }
    
    [HttpDelete("events/{eventId:guid}")]
    public IActionResult DeleteEvent()
    {
        return Ok();
    }
}