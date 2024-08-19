using System.Collections;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record GroupsList(IEnumerable<GroupShort> Groups): IApiResponse;

public record GroupShort(Guid Id, string Name, string ProfilePicture, DateTime CreatedAt);

public record GroupFull(Guid Id, string Name, string ProfilePicture, DateTime CreatedAt): IApiResponse;

public record MembersList(IEnumerable<Member> Members) : IApiResponse;

public record Member(Guid MemberId, string DisplayName);

public record ExternalInvite(Guid InviteId): IApiResponse;
