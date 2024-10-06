using RpgCalendar.Tools.Primitives;

namespace RpgCalendar.Tools.DbLinqExtensions;

public static class TimeRangeExtensions
{
    public static IQueryable<T> WhereOverlapsTimeRange<T>(this IQueryable<T> queryable, DateTime start, DateTime end) where T : ITimeRange
    {
        return queryable.Where(x => (x.StartTime < start && start == x.EndTime)
            || (x.StartTime < end && end < x.EndTime)
            || (start < x.StartTime && x.StartTime < end)
            || (start < x.EndTime && x.EndTime < end));
    }
    
    public static IQueryable<T> WhereNotOverlapsTimeRange<T>(this IQueryable<T> queryable, DateTime start, DateTime end) where T : ITimeRange
    {
        return queryable.Where(x => !((x.StartTime < start && start == x.EndTime)
            || (x.StartTime < end && end < x.EndTime)
            || (start < x.StartTime && x.StartTime < end)
            || (start < x.EndTime && x.EndTime < end)));
    }
}