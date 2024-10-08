﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Groups;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Groups;

[Authorize]
[ApiController, Route("/groups/{groupId:guid}")]
public class GroupController(AccessTester tester,
    Lazy<GetGroupJob> getGroupJob,
    Lazy<PatchGroupJob> patchGroupJob,
    Lazy<DeleteGroupJob> deleteGroupJob,
    Lazy<PatchMembersLimitsJob> patchMembersLimitsJob) : CustomController
{
    [HttpGet]
    public IActionResult GetGroupDetails([FromRoute] Guid groupId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Validate()) return Forbid();

        getGroupJob.Value.Execute(new GetGroupJob.JobData(groupId, Invoker.Id));
        return HandleJobResult(getGroupJob.Value);
    }

    [HttpPatch]
    public IActionResult PatchGroup([FromRoute] Guid groupId, [FromBody] PatchGroup payload)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();
        if (!payload.HasChanges) return EarlyError(ErrorCode.NoChangesRequested);
        
        patchGroupJob.Value.Execute(new PatchGroupJob.JobData(groupId, Invoker.Id, payload.Name, payload.ProfilePicture));
        return HandleJobResult(patchGroupJob.Value);
    }

    [HttpDelete]
    public IActionResult DeleteGroup([FromRoute] Guid groupId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.Group(groupId).Ownership()) return Forbid();

        deleteGroupJob.Value.Execute(new DeleteGroupJob.JobData(groupId));
        return HandleJobResult(deleteGroupJob.Value);
    }
    
    [HttpPatch("limits")]
    public IActionResult UpdateLimits([FromRoute] Guid groupId, [FromBody] PatchMembersLimit limit)
    {
        if (!Privileged) return Forbid();

        patchMembersLimitsJob.Value.Execute(new PatchMembersLimitsJob.JobData(groupId, limit.Limit));
        return HandleJobResult(patchMembersLimitsJob.Value);
    }
}