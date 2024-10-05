using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class AddGroupEventJob(GroupEventService service): IJob
{
    public record JobData(Guid GroupId, Guid CreatorId, string Title, string Description, DateTime Start, DateTime End,
        string? Location, bool? IsOnline);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        Error = service.AddEvent(data.GroupId, data.CreatorId, data.Title, data.Description, data.Start, data.End,
            data.Location, data.IsOnline);

        ApiResponse = service.GetFullApiModel();
    }
}