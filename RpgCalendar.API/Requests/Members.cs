using System.ComponentModel.DataAnnotations;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Requests;

public record InviteMember([Length(6, 6)] string PrivateCode);

public record UpdateGroupPermission(PermissionLevel Permission);