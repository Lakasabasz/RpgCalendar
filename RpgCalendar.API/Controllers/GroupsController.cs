using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Groups;
using RpgCalendar.Commands.Jobs.Users;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers;

[Authorize]
[ApiController, Route("/groups")]
public class GroupsController(
    AccessTester tester,
    Lazy<GetGroupsJob> getGroupsJob,
    Lazy<CreateGroupJob> createGroupJob,
    Lazy<JoinGroupJob> joinGroupJob) : CustomController
{
    [HttpGet]
    public IActionResult GetUserGroups()
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);

        getGroupsJob.Value.Execute(new GetGroupsJob.JobData(Invoker.Id));
        return HandleJobResult(getGroupsJob.Value);
    }

    [HttpPost]
    public IActionResult AddGroup([FromBody] CreateGroup payload)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        
        createGroupJob.Value.Execute(new CreateGroupJob.JobData(Invoker.Id, payload.Name, payload.ProfilePicture));
        return HandleJobResult(createGroupJob.Value);
    }

    [HttpPost("join/{inviteId:guid}")]
    public IActionResult JoinGroup([FromRoute] Guid inviteId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);

        joinGroupJob.Value.Execute(new JoinGroupJob.JobData(Invoker.Id, inviteId));
        return HandleJobResult(joinGroupJob.Value);
    }
}