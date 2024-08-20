using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Groups;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers;

[Authorize]
[ApiController, Route("/groups")]
public class GroupsController(
    AccessTester tester,
    Lazy<GetGroupsJob> getGroupsJob,
    Lazy<CreateGroupJob> createGroupJob) : CustomController
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
}