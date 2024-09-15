using System.ComponentModel.DataAnnotations;
using RpgCalendar.API.Validation;

namespace RpgCalendar.API.Requests;

public record EventsTimePagination(DateTime From, DateTime To): ITimeRangeValidation
{
    public DateTime Begin => From;
    public DateTime End => To;
}

public record EventsCreationRequest([Length(1, 64)] string Title, [MaxLength(1024)] string Description,
    DateTime Start, DateTime End, [MaxLength(32)] string? Location, bool? IsOnline): ITimeRangeValidation
{
    public DateTime Begin => Start;
}

public record EventsPatchRequest([Length(1, 64)] string? Title, [MaxLength(1024)] string? Description,
    DateTime? Start, DateTime? End, [MaxLength(32)] string? Location, bool? IsOnline, [MaxLength(1024)] string? Summary)
    : ITimeRangeValidation
{
    DateTime ITimeRangeValidation.Begin => Start ?? throw new ArgumentNullException(nameof(Start));
    DateTime ITimeRangeValidation.End => End ?? throw new ArgumentNullException(nameof(End));
}