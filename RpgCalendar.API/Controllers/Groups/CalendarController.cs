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
    Lazy<AddGroupEventJob> addGroupEvent): CustomController
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
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();

        addGroupEvent.Value.Execute(new AddGroupEventJob.JobData(groupId, Invoker.Id, payload.Title, payload.Description,
            payload.Start, payload.End, payload.Location, payload.IsOnline));
        return HandleJobResult(addGroupEvent.Value);
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