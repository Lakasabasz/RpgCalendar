namespace RpgCalendar.API.Validation;

public interface ITimeRangeValidation
{
    DateTime Begin { get; }
    DateTime End { get; }
}

public static class TimeRangeValidationExtensions
{
    public static bool ValidateTimeRange(this ITimeRangeValidation validation)
    {
        return validation.Begin <= validation.End;
    }
}