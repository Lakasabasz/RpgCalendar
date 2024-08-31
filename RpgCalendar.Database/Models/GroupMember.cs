using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using RpgCalendar.Tools;

namespace RpgCalendar.Database.Models;

[PrimaryKey(nameof(UserId), nameof(GroupId))]
public class GroupMember
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    public PermissionLevel PermissionLevel { get; set; }
    public GroupUserProfilePicture HeroPicture { get; set; }
    [MaxLength(32)] public string Class { get; set; } = null!;
    [MaxLength(32)] public string Race { get; set; } = null!;
    [MaxLength(32)] public string Location { get; set; } = null!;

    public static GroupMember Prepare(Guid userId, Guid groupId, PermissionLevel level, GroupUserProfilePicture heroPicture,
        string @class, string race, string location)
        => new GroupMember()
        {
            UserId = userId,
            GroupId = groupId,
            PermissionLevel = level,
            HeroPicture = heroPicture,
            Class = @class,
            Race = race,
            Location = location
        };
}