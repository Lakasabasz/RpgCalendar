using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.DTOs;
using RpgCalendar.Commands.Jobs.Users.PrivateCalendar;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Users;

[Authorize]
[ApiController, Route("/users/{userId:guid}/calendar")]
public class CalendarController(AccessTester tester, Lazy<GetAbsencesJob> absencesJob) : CustomController
{
    [HttpGet("absences")]
    public IActionResult GetAbsences([FromRoute] Guid userId, [FromQuery] PrivateCalendarQuery query)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId)) Forbid();

        TimePagination pagination = new TimePagination(query.FromDate, query.FromTime, query.ToDate, query.ToTime);
        if (!pagination.Validate) return EarlyError(ErrorCode.InvalidTimePagination);

        absencesJob.Value.Execute(pagination, new GetAbsencesJob.JobData(userId));
        return HandleJobResult(absencesJob.Value);
    }
    
    [HttpGet("events")]
    public IActionResult GetEvents()
    {
        return Ok();
    }

    [HttpPost]
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