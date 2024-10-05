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
public class CalendarController(AccessTester tester, 
    Lazy<GetAbsencesJob> absencesJob, 
    Lazy<GetEventsJob> getEventsJob,
    Lazy<AddEventJob> addEventJob,
    Lazy<PatchEventJob> patchEventJob,
    Lazy<DeleteEventJob> deleteEventJob) : CustomController
{
    [HttpGet("absences")]
    public IActionResult GetAbsences([FromRoute] Guid userId, [FromQuery] PrivateCalendarQuery query)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId).Validate()) Forbid();

        TimePagination pagination = new TimePagination(query.FromDate, query.FromTime, query.ToDate, query.ToTime);
        if (!pagination.Validate) return EarlyError(ErrorCode.InvalidTimeRange);

        absencesJob.Value.Execute(pagination, new GetAbsencesJob.JobData(userId));
        return HandleJobResult(absencesJob.Value);
    }
    
    [HttpGet("events")]
    public IActionResult GetEvents([FromRoute] Guid userId, [FromQuery] PrivateCalendarQuery query)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId).Validate()) Forbid();

        TimePagination pagination = new TimePagination(query.FromDate, query.FromTime, query.ToDate, query.ToTime);
        if (!pagination.Validate) return EarlyError(ErrorCode.InvalidTimeRange);

        getEventsJob.Value.Execute(pagination, new GetEventsJob.JobData(userId));
        return HandleJobResult(getEventsJob.Value);
    }

    [HttpPost]
    public IActionResult AddEvent([FromRoute] Guid userId, [FromBody] PrivateCalendarAddEvent payload)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId).Validate()) Forbid();

        if (new DateTime(payload.StartingDay, payload.StartingHour) > new DateTime(payload.EndingDay, payload.EndingHour))
            return EarlyError(ErrorCode.InvalidTimeRange);

        addEventJob.Value.Execute(new AddEventJob.JobData(userId, payload.Title, payload.Description,
            payload.StartingDay, payload.StartingHour, payload.EndingDay, payload.EndingHour,
            payload.IsOnline, payload.Location, payload.OverwriteApprovals));
        return HandleJobResult(addEventJob.Value);
    }
    
    [HttpPatch("events/{eventId:guid}")]
    public IActionResult UpdateEvent([FromRoute] Guid userId, [FromRoute] Guid eventId,
        [FromBody] PrivateCalendarPatchEvent payload)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId).Event(eventId)) Forbid();

        if (!payload.HasChange) return EarlyError(ErrorCode.NoChangesRequested);

        patchEventJob.Value.Execute(new PatchEventJob.JobData(eventId, payload.Title, payload.Description,
            payload.StartingDay, payload.StartingHour, payload.EndingDay, payload.EndingHour, payload.IsOnline,
            payload.Location));
        return HandleJobResult(patchEventJob.Value);
    }
    
    [HttpDelete("events/{eventId:guid}")]
    public IActionResult DeleteEvent([FromRoute] Guid userId, [FromRoute] Guid eventId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId).Event(eventId)) Forbid();

        deleteEventJob.Value.Execute(new DeleteEventJob.JobData(eventId));
        return HandleJobResult(deleteEventJob.Value);
    }
}