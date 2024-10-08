﻿namespace RpgCalendar.Tools;

public static partial class Consts
{
    public static class JwtConsts
    {
        public const string UserId = "userid";
        public const string Privileged = "privileged";
    }

    public static class AuthConsts
    {
        public const string UserContextField = "UserContext";
        public const string Privileged = "Privileged";
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
            [ErrorCode.CannotRemoveOwner] = new ErrorApiModel(ErrorCode.CannotRemoveOwner, "Cannot remove owner from its group"),
            [ErrorCode.MemberNotInGroup] = new ErrorApiModel(ErrorCode.MemberNotInGroup, "Member already not in group"),
            [ErrorCode.CannotSetOwnerPermission] = new ErrorApiModel(ErrorCode.CannotSetOwnerPermission, "This endpoint cannot be used to set owner permission"),
            [ErrorCode.CannotChangeOwnerPermission] = new ErrorApiModel(ErrorCode.CannotChangeOwnerPermission, "Cannot remove owner permission from its group"),
            [ErrorCode.OwnedGroupsLimitReached] = new ErrorApiModel(ErrorCode.OwnedGroupsLimitReached, "Owned groups limit reached"),
            [ErrorCode.MembersLimitReached] = new ErrorApiModel(ErrorCode.MembersLimitReached, "Members limit reached"),
            [ErrorCode.GroupNotExists] = new ErrorApiModel(ErrorCode.GroupNotExists, "Group not exists"),
            [ErrorCode.CannotSelfBlock] = new ErrorApiModel(ErrorCode.CannotSelfBlock, "Cannot block yourself"),
            [ErrorCode.UserAlreadyBlacklisted] = new ErrorApiModel(ErrorCode.UserAlreadyBlacklisted, "User already blacklisted"),
            [ErrorCode.BlacklistIdInvalid] = new ErrorApiModel(ErrorCode.BlacklistIdInvalid, "Provided id could not be match to group or user"),
            [ErrorCode.GroupAlreadyBlacklisted] = new ErrorApiModel(ErrorCode.GroupAlreadyBlacklisted, "Group already blacklisted"),
            [ErrorCode.CannotBlacklistOwnGroup] = new ErrorApiModel(ErrorCode.CannotBlacklistOwnGroup, "Cannot blacklist group you are member of"),
            [ErrorCode.CannotJoinBlacklistedGroup] = new ErrorApiModel(ErrorCode.CannotJoinBlacklistedGroup, "Cannot join blacklisted group"),
            [ErrorCode.UserBlacklistedInvoker] = new ErrorApiModel(ErrorCode.UserBlacklistedInvoker, "Cannot add user, because invoker is blocked"),
            [ErrorCode.NoRelationChange] = new ErrorApiModel(ErrorCode.NoRelationChange, "Cannot change relation towards event with the same status"),
            [ErrorCode.PatchOverlappingEvent] = new ErrorApiModel(ErrorCode.PatchOverlappingEvent, "Patch leads to overlapping events"),
            [ErrorCode.CannotAddOverlappingEvent] = new ErrorApiModel(ErrorCode.CannotAddOverlappingEvent, "Event overlaps with existing events"),
            [ErrorCode.StartDateCannotBePast] = new ErrorApiModel(ErrorCode.StartDateCannotBePast, "Cannot create event that starts in the past"),
            [ErrorCode.CannotEditFinishedEvents] = new ErrorApiModel(ErrorCode.CannotEditFinishedEvents, "Cannot edit finished events"),
            [ErrorCode.OverwriteApprovalRequiredForTimeChanging] = new ErrorApiModel(ErrorCode.OverwriteApprovalRequiredForTimeChanging, 
                "OverwriteApproval field is required for event date and time changes"),
        };

        public static ErrorApiModel FallbackErrorMessage(ErrorCode errorCode)
            => new (ErrorCode.SomethingWentWrong, $"Unhandled error code {errorCode}");
    }
}