using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class Group
{
    [Key] public Guid GroupId { get; set; }

    [MaxLength(32)] public string Name { get; set; } = null!;
    
    public Guid? ProfilePicture { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public Guid OwnerId { get; set; }

    public User Owner { get; set; } = null!;

    public static Group Prepare(Guid ownerId, string name, Guid? profilePicture)
    {
        return new Group()
        {
            GroupId = Guid.NewGuid(),
            Name = name,
            CreationDate = DateTime.Now,
            ProfilePicture = profilePicture,
            OwnerId = ownerId
        };
    }
}