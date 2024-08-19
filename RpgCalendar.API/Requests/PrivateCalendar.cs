namespace RpgCalendar.API.Requests;

public record PrivateCalendarQuery(DateOnly? FromDate, TimeOnly? FromTime, DateOnly? ToDate, TimeOnly? ToTime);