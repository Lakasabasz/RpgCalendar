using System.ComponentModel.DataAnnotations;
using RpgCalendar.Tools.Primitives;

namespace RpgCalendar.Database.Models;

public class PrivateEvent: ITimeRange
{
    [Key] public Guid EventId { get; set; }
    
    [MaxLength(256)] public string? Title { get; set; }
    
    [MaxLength(1024)] public string? Description { get; set; }
    
    public bool SimpleAbsence { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public Guid OwnerId { get; set; }

    public virtual User Owner { get; set; } = null!;
    
    public bool IsOnline { get; set; }

    [MaxLength(128)] public string? Location { get; set; }

    public static PrivateEvent Prepare(string? title, string? description, bool simpleAbsence,
        DateTime start, DateTime end, Guid ownerId, bool isOnline, string? location) => new()
    {
        EventId = Guid.NewGuid(),
        Title = title,
        Description = description,
        SimpleAbsence = simpleAbsence,
        EndTime = end,
        StartTime = start,
        OwnerId = ownerId,
        IsOnline = isOnline,
        Location = location
    };
}