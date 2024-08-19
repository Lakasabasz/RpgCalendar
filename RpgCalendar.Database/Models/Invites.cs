using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class Invites
{
    [Key] public Guid InviteId { get; set; }
    
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
}