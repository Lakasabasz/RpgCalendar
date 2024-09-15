using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands;

public class UserScope(RelationalDb db, ILogger<AccessTester> logger, User? invoker, Guid targetUserId)
{
    public bool Validate()
    {
        if (invoker?.Id == targetUserId) return true;
        logger.LogInformation("Invoker ({InvokerId}:{InvokerNick}) has no access to target user ({Target})",
            invoker?.Id, invoker?.Nick, targetUserId);
        return false;
    }

    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataQuery
    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
    public bool Event(Guid eventId)
    {
        var @event = db.PrivateEvents.FirstOrDefault(x => x.EventId == eventId);
        if (Validate() && @event?.OwnerId == invoker?.Id) return true;
        logger.LogInformation("Invoker ({InvokerId}:{InvokerNick}) has no access to target event ({Target})",
            invoker?.Id, invoker?.Nick, eventId);
        return false;
    }
}

public class GroupScope(RelationalDb db, ILogger<AccessTester> logger, User? invoker, Guid targetGroupId)
{
    public bool Validate()
    {
        var invokerId = invoker?.Id;
        var membership = db.GroupsMembers
            .FirstOrDefault(x => x.GroupId == targetGroupId && x.UserId == invokerId);
        if (membership is not null) return true;
        logger.LogInformation("Invoker ({InvokerId}:{InvokerNick}) has no access to target group ({Target})",
            invoker?.Id, invoker?.Nick, targetGroupId);
        return false;
    }

    public bool Manage()
    {
        var invokerId = invoker?.Id;
        var group = db.Groups.FirstOrDefault(x => x.GroupId == targetGroupId);
        var membership = db.GroupsMembers.FirstOrDefault(x => x.GroupId == targetGroupId && x.UserId == invokerId);
        if (Validate() && (group?.OwnerId == invokerId || membership?.PermissionLevel == PermissionLevel.Admin)) return true;
        logger.LogInformation("Invoker ({InvokerId}:{InvokerNick}) has no access to manage target group ({Target})",
            invoker?.Id, invoker?.Nick, targetGroupId);
        return false;
    }

    public bool Ownership()
    {
        var invokerId = invoker?.Id;
        var group = db.Groups.FirstOrDefault(x => x.GroupId == targetGroupId);
        if (Validate() && group?.OwnerId == invokerId) return true;
        logger.LogInformation("Invoker ({InvokerId}:{InvokerNick}) has no access to owner permissions in target group ({Target})",
            invoker?.Id, invoker?.Nick, targetGroupId);
        return false;
    }

    public bool Event(Guid eventId)
    {
        var groupEvent = db.GroupEvents.FirstOrDefault(x => x.GroupEventId == eventId);
        if(Validate() && groupEvent?.GroupId == targetGroupId) return true;
        logger.LogInformation("Invoker ({InvokerId}:{InvokerNick}) has no access to event ({Target})",
            invoker?.Id, invoker?.Nick, eventId);
        return false;
    }
}

public class AccessScope(RelationalDb db, ILogger<AccessTester> logger, User? invoker)
{
    public UserScope User(Guid userId) => new UserScope(db, logger, invoker, userId);

    public GroupScope Group(Guid groupId) => new GroupScope(db, logger, invoker, groupId);
}

public class AccessTester(RelationalDb db, ILogger<AccessTester> logger)
{
    private User? _invoker;
    public AccessScope HasAccessTo => new (db, logger, _invoker);

    public AccessTester TestIf(User? invoker)
    {
        _invoker = invoker;
        return this;
    }
}