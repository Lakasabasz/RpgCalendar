using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.Database.Models;

public class User
{
    [Key] public Guid Id { get; set; }
    
    [MaxLength(64)] public string Nick { get; set; }
    
    [MaxLength(6)] public string PrivateCode { get; set; }
}