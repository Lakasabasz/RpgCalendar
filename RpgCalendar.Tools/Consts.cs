namespace RpgCalendar.Tools;

public static partial class Consts
{
    public static class JwtConsts
    {
        public const string UserId = "userid";
    }

    public static class AuthConsts
    {
        public const string UserContextField = "UserContext";
    }

    public static class Errors
    {
        public static readonly Dictionary<ErrorCode, ErrorApiModel> ErrorCodeMessages = new()
        {
            [ErrorCode.UserNotRegistered] = new ErrorApiModel(ErrorCode.UserNotRegistered, "The user is not registered."),
            [ErrorCode.UserAlreadyRegistered] = new ErrorApiModel(ErrorCode.UserAlreadyRegistered, "The user is already registered."),
            [ErrorCode.InvalidTimeRange] = new ErrorApiModel(ErrorCode.InvalidTimeRange, "Provided time range is negative."),
            [ErrorCode.TitleAndDescriptionMismatch] = new ErrorApiModel(ErrorCode.TitleAndDescriptionMismatch, "Title and description must be provided both or not provided at all"),
            [ErrorCode.NoChangesRequested] = new ErrorApiModel(ErrorCode.NoChangesRequested, "Patch changes nothing"),
            [ErrorCode.CannotChangeEventType] = new ErrorApiModel(ErrorCode.CannotChangeEventType, "Patch cannot remove or add title or description"),
            [ErrorCode.PatchInvalidTimeRange] = new ErrorApiModel(ErrorCode.PatchInvalidTimeRange, "Patch change leads to invalid time range"),
            [ErrorCode.ImageNotExists] = new ErrorApiModel(ErrorCode.ImageNotExists, "Chosen image found in storage"),
            [ErrorCode.UserNotExists] = new ErrorApiModel(ErrorCode.UserNotExists, "User not exists"),
            [ErrorCode.UserAlreadyInGroup] = new ErrorApiModel(ErrorCode.UserAlreadyInGroup, "User is already in group"),
            [ErrorCode.InviteNotExists] = new ErrorApiModel(ErrorCode.InviteNotExists, "Invite not exists"),
            [ErrorCode.CannotRemoveOwner] = new ErrorApiModel(ErrorCode.CannotRemoveOwner, "Cannot remove owner from it's group"),
            [ErrorCode.MemberNotInGroup] = new ErrorApiModel(ErrorCode.MemberNotInGroup, "Member already not in group"),
            [ErrorCode.CannotSetOwnerPermission] = new ErrorApiModel(ErrorCode.CannotSetOwnerPermission, "This endpoint cannot be used to set owner permission"),
            [ErrorCode.CannotChangeOwnerPermission] = new ErrorApiModel(ErrorCode.CannotChangeOwnerPermission, "Cannot remove owner permission from it's group"),
            [ErrorCode.OwnedGroupsLimitReached] = new ErrorApiModel(ErrorCode.OwnedGroupsLimitReached, "Owned groups limit reached"),
            [ErrorCode.MembersLimitReached] = new ErrorApiModel(ErrorCode.MembersLimitReached, "Members limit reached"),
        };

        public static ErrorApiModel FallbackErrorMessage(ErrorCode errorCode)
            => new (ErrorCode.SomethingWentWrong, $"Unhandled error code {errorCode}");
    }
}