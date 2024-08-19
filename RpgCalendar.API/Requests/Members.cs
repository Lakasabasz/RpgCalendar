using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.API.Requests;

public record InviteMember([Length(6, 6)] string PrivateCode);