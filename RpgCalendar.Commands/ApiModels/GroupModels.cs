using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands.ApiModels;

public record GroupsList(IEnumerable<GroupModel> Groups): IApiResponse;

public record GroupShort(Guid Id, string Name);
    
public record GroupModel(Guid Id, string Name, string ProfilePicture, DateTime CreatedAt);

public record GroupFull(Guid Id, Guid OwnerId, string Name, string ProfilePicture,
    DateTime CreatedAt, System System, GroupUserHeroProfile Hero): IApiResponse;

public record GroupUserHeroProfile(GroupUserProfilePicture ProfilePicture, string Class, string Race, string Location);

public record MembersList(IEnumerable<Member> Members, ulong MaxMembers) : IApiResponse;

public record Member(Guid MemberId, string DisplayName, PermissionLevel PermissionLevel);

public record ExternalInvite(Guid InviteId): IApiResponse;
