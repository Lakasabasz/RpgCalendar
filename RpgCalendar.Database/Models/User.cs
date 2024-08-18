using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RpgCalendar.Database.Models;

[Index(nameof(PrivateCode), IsUnique = true)]
public class User
{
    [Key] public Guid Id { get; set; }
    
    [MaxLength(64)] public string Nick { get; set; }
    
    [MaxLength(6)] public string PrivateCode { get; set; }
    
    public static User Prepare(Guid id, string nick, string privateCode) => new()
    {
        Id = id,
        Nick = nick,
        PrivateCode = privateCode
    };
}