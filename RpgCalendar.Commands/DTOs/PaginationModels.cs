namespace RpgCalendar.Commands.DTOs;

public record TimePagination(DateTime Start, DateTime End)
{
    public TimePagination(DateOnly? fromDate, TimeOnly? fromTime, DateOnly? toDate, TimeOnly? toTime)
        : this(new DateTime(fromDate ?? DefaultFromDate, fromTime ?? DefaultFromTime),
            new DateTime(toDate ?? DefaultToDate, toTime ?? DefaultToTime)){ }

    public bool Validate => End >= Start;
    
    private static DateOnly DefaultFromDate => DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
    private static TimeOnly DefaultFromTime => new (0, 0);
    
    private static DateOnly DefaultToDate => DateOnly.FromDateTime(DateTime.Now);
    private static TimeOnly DefaultToTime => new (23, 59);
}