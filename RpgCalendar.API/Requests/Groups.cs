using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.API.Requests;

public record CreateGroup([MaxLength(32), MinLength(3)] string Name, Guid ProfilePicture);

public record PatchGroup([MaxLength(32), MinLength(3)] string? Name, Guid? ProfilePicture);