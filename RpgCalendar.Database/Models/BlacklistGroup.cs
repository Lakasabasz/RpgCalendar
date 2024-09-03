using Microsoft.EntityFrameworkCore;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(EntryOwnerId), nameof(BlacklistedGroupId))]
public class BlacklistGroup
{
    public Guid EntryOwnerId { get; set; }
    public User EntryOwner { get; set; }
    
    public Guid BlacklistedGroupId { get; set; }
    public Group BlacklistedGroup { get; set; }

    public static BlacklistGroup Prepare(Guid ownerId, Guid groupId) =>
        new()
        {
            EntryOwnerId = ownerId,
            BlacklistedGroupId = groupId
        };
}