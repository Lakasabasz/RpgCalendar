using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Blacklist;

public class GetBlacklistJob(RelationalDb db): IJob
{
    public record JobData(Guid UserId);

    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var users = db.BlacklistUsers.Where(x => x.EntryOwnerId == data.UserId)
            .Include(x => x.BlacklistedUser)
            .Select(x => new UserShort(x.BlacklistedUser.Id, x.BlacklistedUser.Nick));
        var groups = db.BlacklistGroups.Where(x => x.EntryOwnerId == data.UserId)
            .Include(x => x.BlacklistedGroup)
            .Select(x => new GroupShort(x.BlacklistedGroup.GroupId, x.BlacklistedGroup.Name));

        ApiResponse = new BlacklistModel(users, groups);
    }
}