using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class GroupEvent
{
    [Key] public Guid GroupEventId { get; set; }
    
    public Guid GroupId { get; set; }
    public Group Group { get; set; } = null!;
    
    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = null!;
    
    [MaxLength(64)] public string Title { get; set; } = null!;
    
    [MaxLength(1024)] public string Description { get; set; } = null!;
    
    [MaxLength(1024)] public string? Summary { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    [MaxLength(32)] public string? Location { get; set; }
    
    public bool? IsOnline { get; set; }

    public static GroupEvent Prepare(Guid creatorId, Guid groupId, string title, string description, DateTime start,
        DateTime end, string? location, bool? isOnline)
    {
        return new GroupEvent()
        {
            GroupEventId = Guid.NewGuid(),
            CreatorId = creatorId,
            GroupId = groupId,
            Title = title,
            Description = description,
            StartTime = start,
            EndTime = end,
            Location = location,
            IsOnline = isOnline
        };
    }
}