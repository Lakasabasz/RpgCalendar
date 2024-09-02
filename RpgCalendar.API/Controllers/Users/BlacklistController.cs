using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands;
using RpgCalendar.Commands.Jobs.Blacklist;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers.Users;

[Authorize]
[ApiController, Route("/users/{userId:guid}/blacklist")]
public class BlacklistController(AccessTester tester,
    Lazy<GetBlacklistJob> getBlacklistJob,
    Lazy<BlacklistEntryJob> blacklistEntryJob,
    Lazy<RemoveBlacklistEntryJob> removeBlacklistEntryJob) : CustomController
{
    [HttpGet]
    public IActionResult GetBlacklist([FromRoute] Guid userId)
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if (!tester.TestIf(Invoker).HasAccessTo.User(userId).Validate()) return Forbid();
        
        getBlacklistJob.Value.Execute(new GetBlacklistJob.JobData(userId));
        return HandleJobResult(getBlacklistJob.Value);
    }
    
    [HttpPost]
    public IActionResult BlacklistEntry([FromRoute] Guid userId, [FromBody] BlacklistRequest payload)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!tester.TestIf(Invoker).HasAccessTo.User(userId).Validate()) return Forbid();
        if (userId == payload.BlacklistId) return EarlyError(ErrorCode.CannotSelfBlock);
        
        blacklistEntryJob.Value.Execute(new BlacklistEntryJob.JobData(userId, payload.BlacklistId));
        return HandleJobResult(blacklistEntryJob.Value);
    }
    
    [HttpDelete("{blacklistId:guid}")]
    public IActionResult DeleteBlacklist([FromRoute] Guid userId, [FromRoute] Guid blacklistId)
    {
        if(Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        if(!tester.TestIf(Invoker).HasAccessTo.User(userId).Validate()) return Forbid();
        
        removeBlacklistEntryJob.Value.Execute(new RemoveBlacklistEntryJob.JobData(userId, blacklistId));
        return HandleJobResult(removeBlacklistEntryJob.Value);
    }
}