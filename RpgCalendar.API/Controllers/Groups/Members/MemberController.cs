﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Groups.Members;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Groups.Members;

[ApiController]
[Authorize, Route("groups/{groupId:guid}/members/{memberId:guid}")]
class MemberController(AccessTester tester,
    Lazy<PatchMemberPermissionJob> patchMemberPermission,
    Lazy<TransferOwnershipJob> transferOwnership): CustomController
{
    [HttpPatch("permission")]
    public IActionResult UpdateGroupMemberPermission([FromRoute] Guid groupId, [FromRoute] Guid memberId, 
                                                     [FromBody] UpdateGroupPermission payload)
    {
        if (payload.Permission == PermissionLevel.Owner) return EarlyError(ErrorCode.CannotSetOwnerPermission);
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (tester.TestIf(Invoker).HasAccessTo.Group(groupId).Manage()) return Forbid();
        
        patchMemberPermission.Value.Execute(new PatchMemberPermissionJob.JobData(groupId, memberId, payload.Permission));
        return HandleJobResult(patchMemberPermission.Value);
    }
    
    [HttpPatch("ownership")]
    public IActionResult TransferOwnership([FromRoute] Guid groupId, [FromRoute] Guid memberId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (tester.TestIf(Invoker).HasAccessTo.Group(groupId).Ownership()) return Forbid();
        
        transferOwnership.Value.Execute(new TransferOwnershipJob.JobData(groupId, memberId, Invoker.Id));
        return HandleJobResult(transferOwnership.Value);
    }
}