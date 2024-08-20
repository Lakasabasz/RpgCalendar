﻿namespace RpgCalendar.Tools;

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
    ImageNotExists = 8
}