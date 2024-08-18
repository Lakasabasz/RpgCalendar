﻿namespace RpgCalendar.Tools;

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
        };

        public static ErrorApiModel FallbackErrorMessage(ErrorCode errorCode)
            => new (ErrorCode.SomethingWentWrong, $"Unhandled error code {errorCode}");
    }
}