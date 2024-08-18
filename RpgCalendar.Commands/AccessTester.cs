using Microsoft.Extensions.Logging;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;

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
}

public class AccessScope(RelationalDb db, ILogger<AccessTester> logger, User? invoker)
{
    public bool User(Guid userId) => new UserScope(db, logger, invoker, userId).Validate();
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

    public bool HasAccess()
    {
        throw new NotImplementedException();
    }
}