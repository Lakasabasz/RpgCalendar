using Microsoft.EntityFrameworkCore;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(EntryOwnerId), nameof(BlacklistedGroupId))]
public class BlacklistGroup
{
    public Guid EntryOwnerId { get; set; }
    public User EntryOwner { get; set; }
    
    public Guid BlacklistedGroupId { get; set; }
    public Group BlacklistedGroup { get; set; }
}