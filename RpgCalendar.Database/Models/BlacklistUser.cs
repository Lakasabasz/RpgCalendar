using Microsoft.EntityFrameworkCore;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(EntryOwnerId), nameof(BlacklistedUserId))]
public class BlacklistUser
{
    public Guid EntryOwnerId { get; set; }
    public User EntryOwner { get; set; }
    
    public Guid BlacklistedUserId { get; set; }
    public User BlacklistedUser { get; set; }
}