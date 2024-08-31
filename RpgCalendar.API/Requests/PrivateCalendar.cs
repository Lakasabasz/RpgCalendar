using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.API.Requests;

public record PrivateCalendarQuery(DateOnly? FromDate, TimeOnly? FromTime, DateOnly? ToDate, TimeOnly? ToTime);

public record PrivateCalendarAddEvent(
    [MaxLength(256), MinLength(3)] string? Title,
    [MaxLength(1024), MinLength(3)] string? Description,
    DateOnly StartingDay, TimeOnly StartingHour,
    DateOnly EndingDay, TimeOnly EndingHour,
    bool IsOnline, [MaxLength(128)] string? Location);

public record PrivateCalendarPatchEvent(
    [MaxLength(256), MinLength(3)] string? Title,
    [MaxLength(1024), MinLength(3)] string? Description,
    DateOnly? StartingDay, TimeOnly? StartingHour,
    DateOnly? EndingDay, TimeOnly? EndingHour,
    bool? IsOnline, [MaxLength(128)] string? Location)
{
    public bool HasChange => Title is not null
                             || Description is not null
                             || StartingDay.HasValue
                             || StartingHour.HasValue
                             || EndingDay.HasValue
                             || EndingHour.HasValue
                             || IsOnline.HasValue
                             || Location is not null;
}