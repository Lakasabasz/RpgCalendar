using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record AbsencePeriods(IEnumerable<AbsencePeriod> Absences): IApiResponse;

public record AbsencePeriod(DateOnly StartingDay, TimeOnly StartingHour, DateOnly EndingDay, TimeOnly EndingOnly)
{
    public static AbsencePeriod FromDateTime(DateTime start, DateTime end)
        => new AbsencePeriod(
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end));
}

public record PrivateEvents(IEnumerable<PrivateEvent> Events): IApiResponse;

public record PrivateEvent(string Title, string Description, DateOnly StartingDay, TimeOnly StartingHour, DateOnly EndingDay, TimeOnly EndingOnly)
{
    public static PrivateEvent FromDateTime(string title, string description, DateTime start, DateTime end)
        => new PrivateEvent(title, description,
            DateOnly.FromDateTime(start), TimeOnly.FromDateTime(start),
            DateOnly.FromDateTime(end), TimeOnly.FromDateTime(end));
}