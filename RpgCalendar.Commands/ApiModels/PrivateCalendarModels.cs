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

public record PrivateEvents(IEnumerable<PrivateEvent> Events): IApiResponse;

public record PrivateEvent(Guid EventId, string Title, string Description,
    DateOnly StartingDay, TimeOnly StartingHour, DateOnly EndingDay, TimeOnly EndingOnly)
{
    public static PrivateEvent FromDateTime(Guid eventId, string title, string description, DateTime start, DateTime end)
        => new PrivateEvent(eventId, title, description,
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end));
}

public record FullPrivateEvent(Guid EventId, Guid OwnerId, string? Title, string? Description,
    DateOnly StartingDay, TimeOnly StartingHour, DateOnly EndingDay, TimeOnly EndingOnly)
    : IApiResponse
{
    public static FullPrivateEvent FromDateTime(Guid eventId, Guid ownerId, string? title, string? description, DateTime start, DateTime end)
        => new FullPrivateEvent(eventId, ownerId, title, description,
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end));
}