using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;

namespace RpgCalendar.Commands;

public class BlacklistTools
{
    public static BlacklistModel PrepareBlacklistResponse(RelationalDb db, Guid invokerId)
    {
        var users = db.BlacklistUsers.Where(x => x.EntryOwnerId == invokerId)
            .Include(x => x.BlacklistedUser)
            .Select(x => new UserShort(x.BlacklistedUser.Id, x.BlacklistedUser.Nick));
        var groups = db.BlacklistGroups.Where(x => x.EntryOwnerId == invokerId)
            .Include(x => x.BlacklistedGroup)
            .Select(x => new GroupShort(x.BlacklistedGroup.GroupId, x.BlacklistedGroup.Name));

        return new BlacklistModel(users, groups);
    }
}