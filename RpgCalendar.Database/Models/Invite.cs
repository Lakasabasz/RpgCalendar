using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class Invite
{
    [Key] public Guid InviteId { get; set; }
    
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;

    public static Invite Prepare(Guid groupId) => 
        new Invite()
        {
            InviteId = Guid.NewGuid(),
            GroupId = groupId,
        };
}