using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class AddEventJob(PrivateEventService privateEventService): IJob
{
    public record JobData(Guid OwnerId, string? Title, string? Description, DateOnly StartingDay, TimeOnly StartingHour,
        DateOnly EndingDay, TimeOnly EndingHour, bool IsOnline, string? Location, bool OverwriteApprovals);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        Error = privateEventService.AddEvent(data.Title, data.Description, data.StartingDay.ToDateTime(data.StartingHour), data.EndingDay.ToDateTime(data.EndingHour),
            data.OwnerId, data.OverwriteApprovals, data.IsOnline, data.Location);

        ApiResponse = privateEventService.GetFullApiModel();
    }
}