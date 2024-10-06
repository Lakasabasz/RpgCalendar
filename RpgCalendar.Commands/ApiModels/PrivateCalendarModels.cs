using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record AbsencePeriods(IEnumerable<AbsencePeriod> Absences): IApiResponse;

public record AbsencePeriod(Guid EventId, DateOnly StartingDay, TimeOnly StartingHour, 
    DateOnly EndingDay, TimeOnly EndingOnly)
{
    public static AbsencePeriod FromDateTime(Guid eventId, DateTime start, DateTime end)
        => new AbsencePeriod(eventId,
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end));
}

public record GroupRequestedAbsencesModel(IEnumerable<GroupRequestedAbsenceModel> Absences): IApiResponse;

public record GroupRequestedAbsenceModel(IEnumerable<AbsencePeriodSimpleModel> Periods, UserShortWithProfileModel Member);

public record AbsencePeriodSimpleModel(Guid EventId, DateTime Start, DateTime End);

public record PrivateEvents(IEnumerable<PrivateEvent> Events): IApiResponse;

public record PrivateEvent(Guid EventId, string Title, string Description,
    DateOnly StartingDay, TimeOnly StartingHour, DateOnly EndingDay, TimeOnly EndingOnly,
    bool IsOnline, string? Location)
{
    public static PrivateEvent FromDateTime(Guid eventId, string title, string description, DateTime start, DateTime end,
        bool isOnline, string? location)
        => new PrivateEvent(eventId, title, description,
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end),
            isOnline, location);
}

public record FullPrivateEventModel(Guid EventId, Guid OwnerId, string? Title, string? Description,
    DateOnly StartingDay, TimeOnly StartingHour, DateOnly EndingDay, TimeOnly EndingOnly,
    bool IsOnline, string? Location)
    : IApiResponse
{
    public static FullPrivateEventModel FromDateTime(Guid eventId, Guid ownerId, string? title, string? description,
        DateTime start, DateTime end, bool isOnline, string? location)
        => new FullPrivateEventModel(eventId, ownerId, title, description,
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end),
            isOnline, location);
}