using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class PrivateEvent
{
    [Key] public Guid EventId { get; set; }
    
    [MaxLength(256)] public string? Title { get; set; }
    
    [MaxLength(1024)] public string? Description { get; set; }
    
    public bool SimpleAbsence { get; set; }
    
    public DateTime Start { get; set; }
    
    public DateTime End { get; set; }
    
    public Guid OwnerId { get; set; }

    public virtual User Owner { get; set; } = null!;
}