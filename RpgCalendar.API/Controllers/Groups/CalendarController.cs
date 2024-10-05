using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.API.Validation;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Groups.Events;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}/calendar")]
public class CalendarController(AccessTester tester,
    Lazy<GetGroupsEventsJob> getEventList,
    Lazy<AddGroupEventJob> addGroupEvent,
    Lazy<GetGroupEventJob> getGroupEvent,
    Lazy<DeleteGroupEventJob> deleteGroupEvent,
    Lazy<PatchGroupEventJob> patchGroupEvent,
    Lazy<PatchEventApproveJob> patchEventApprove,
    Lazy<PatchEventRejectJob> patchEventReject,
    Lazy<GetAbsencesListJob> getAbsencesList): CustomController
{
    [HttpGet]
    public IActionResult EventLists([FromRoute] Guid groupId, [FromQuery] EventsTimePagination range)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!range.ValidateTimeRange()) return EarlyError(ErrorCode.InvalidTimeRange);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Validate()) return Forbid();

        getEventList.Value.Execute(new GetGroupsEventsJob.JobData(range.From, range.To, groupId, Invoker.Id));
        return HandleJobResult(getEventList.Value);
    }

    [HttpPost]
    public IActionResult AddEvent([FromRoute] Guid groupId, [FromBody] EventsCreationRequest payload)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!payload.ValidateTimeRange()) return EarlyError(ErrorCode.InvalidTimeRange);
        if (payload.Start < DateTime.Now) return EarlyError(ErrorCode.StartDateCannotBePast);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();

        addGroupEvent.Value.Execute(new AddGroupEventJob.JobData(groupId, Invoker.Id, payload.Title, payload.Description,
            payload.Start, payload.End, payload.Location, payload.IsOnline));
        return HandleJobResult(addGroupEvent.Value);
    }

    [HttpGet("{eventId:guid}")]
    public IActionResult EventDetails([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Event(eventId)) return Forbid();

        getGroupEvent.Value.Execute(new GetGroupEventJob.JobData(eventId));
        return HandleJobResult(getGroupEvent.Value);
    }

    [HttpDelete("{eventId:guid}")]
    public IActionResult DeleteEvent([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Event(eventId) ||
           !tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();

        deleteGroupEvent.Value.Execute(new DeleteGroupEventJob.JobData(eventId));
        return HandleJobResult(deleteGroupEvent.Value);
    }

    [HttpPatch("{eventId:guid}")]
    public IActionResult EditEvent([FromRoute] Guid groupId, [FromRoute] Guid eventId, [FromBody] EventsPatchRequest payload)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(payload.Start is not null && payload.End is not null && !payload.ValidateTimeRange())
            return EarlyError(ErrorCode.InvalidTimeRange);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Event(eventId) ||
            !tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();
        
        patchGroupEvent.Value.Execute(new PatchGroupEventJob.JobData(eventId, payload.Title, payload.Description, 
            payload.Location, payload.IsOnline, payload.Summary, payload.Start, payload.End));
        return HandleJobResult(patchGroupEvent.Value);
    }
    
    [HttpPatch("{eventId:guid}/approve")]
    public IActionResult ApproveEvent([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Event(eventId)) return Forbid();

        patchEventApprove.Value.Execute(new PatchEventApproveJob.JobData(Invoker.Id, eventId));
        return HandleJobResult(patchEventApprove.Value);
    }
    
    [HttpPatch("{eventId:guid}/reject")]
    public IActionResult RejectEvent([FromRoute] Guid groupId, [FromRoute] Guid eventId)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Event(eventId)) return Forbid();
        
        patchEventReject.Value.Execute(new PatchEventRejectJob.JobData(Invoker.Id, eventId));
        return HandleJobResult(patchEventReject.Value);
    }
    
    [HttpGet("absences")]
    public IActionResult GetAbsences([FromRoute] Guid groupId, [FromQuery] EventsTimePagination range)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!range.ValidateTimeRange()) return EarlyError(ErrorCode.InvalidTimeRange);
        if(!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();
        
        getAbsencesList.Value.Execute(new GetAbsencesListJob.JobData(groupId, range.From, range.To));
        return HandleJobResult(getAbsencesList.Value);
    }
}