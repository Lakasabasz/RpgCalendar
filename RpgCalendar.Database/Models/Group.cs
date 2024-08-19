using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class Group
{
    [Key] public Guid GroupId { get; set; }

    [MaxLength(32)] public string Name { get; set; } = null!;
    
    public Guid? ProfilePicture { get; set; }
    
    public DateTime CreationDate { get; set; }

    public static Group Prepare(string name, Guid? profilePicture)
    {
        return new Group()
        {
            GroupId = Guid.NewGuid(),
            Name = name,
            CreationDate = DateTime.Now,
            ProfilePicture = profilePicture
        };
    }
}