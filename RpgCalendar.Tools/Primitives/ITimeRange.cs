namespace RpgCalendar.Tools.Primitives;

public interface ITimeRange
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public static class TimeRangeExtensions
{
    public static IQueryable<T> TimeRange<T>(this IQueryable<T> queryable, DateTime start, DateTime end) where T : ITimeRange
    {
        return queryable.Where(x => (x.StartTime < start && start == x.EndTime)
                                       || (x.StartTime < end && end < x.EndTime)
                                       || (start < x.StartTime && x.StartTime < end)
                                       || (start < x.EndTime && x.EndTime < end));
    }
}