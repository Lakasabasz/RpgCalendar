using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RpgCalendar.API.Controllers.Users;

[Authorize]
[ApiController, Route("/users/{userId:guid}/calendar")]
public class CalendarController : CustomController
{
    [HttpGet("absences")]
    public IActionResult GetAbsences()
    {
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