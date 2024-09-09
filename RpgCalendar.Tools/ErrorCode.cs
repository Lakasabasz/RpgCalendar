namespace RpgCalendar.Tools;

public enum ErrorCode
{
    SomethingWentWrong = 0,
    UserNotRegistered = 1,
    UserAlreadyRegistered = 2,
    InvalidTimeRange = 3,
    TitleAndDescriptionMismatch = 4,
    NoChangesRequested = 5,
    CannotChangeEventType = 6,
    PatchInvalidTimeRange = 7,
    ImageNotExists = 8,
    UserNotExists = 9,
    UserAlreadyInGroup = 10,
    InviteNotExists = 11,
    CannotRemoveOwner = 12,
    MemberNotInGroup = 13,
    CannotSetOwnerPermission = 14,
    CannotChangeOwnerPermission = 15,
    OwnedGroupsLimitReached = 16,
    MembersLimitReached = 17,
    GroupNotExists = 18,
    CannotSelfBlock = 19,
    UserAlreadyBlacklisted = 20,
    BlacklistIdInvalid = 21,
    GroupAlreadyBlacklisted = 22,
    CannotBlacklistOwnGroup = 23,
    CannotJoinBlacklistedGroup = 24,
    UserBlacklistedInvoker = 25
}