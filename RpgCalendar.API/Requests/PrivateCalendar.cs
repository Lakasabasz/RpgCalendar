using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.API.Requests;

public record PrivateCalendarQuery(DateOnly? FromDate, TimeOnly? FromTime, DateOnly? ToDate, TimeOnly? ToTime);

public record PrivateCalendarAddEvent(
    [MaxLength(256), MinLength(3)] string? Title, 
    [MaxLength(1024), MinLength(3)] string? Description, 
    DateOnly StartingDay, TimeOnly StartingHour,
    DateOnly EndingDay, TimeOnly EndingHour);