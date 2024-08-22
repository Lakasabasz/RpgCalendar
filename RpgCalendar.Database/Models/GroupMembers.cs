using Microsoft.EntityFrameworkCore;
using RpgCalendar.Tools;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class GroupMembers
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    public PermissionLevel PermissionLevel { get; set; }

    public static GroupMembers Prepare(Guid userId, Guid groupId, PermissionLevel level)
        => new GroupMembers()
        {
            UserId = userId,
            GroupId = groupId,
            PermissionLevel = level
        };
}