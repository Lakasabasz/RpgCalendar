using Microsoft.EntityFrameworkCore;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(EntryOwnerId), nameof(BlacklistedUserId))]
public class BlacklistUser
{
    public Guid EntryOwnerId { get; set; }
    public User EntryOwner { get; set; } = null!;
    
    public Guid BlacklistedUserId { get; set; }
    public User BlacklistedUser { get; set; } = null!;

    public static BlacklistUser Prepare(Guid ownerId, Guid userId) =>
        new()
        {
            EntryOwnerId = ownerId,
            BlacklistedUserId = userId
        };
}