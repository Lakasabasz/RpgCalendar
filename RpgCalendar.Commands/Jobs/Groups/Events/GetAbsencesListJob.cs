using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class GetAbsencesListJob(GroupService service): IJob
{
    public record JobData(Guid GroupId, DateTime StartDate, DateTime EndDate);
    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; }

    public void Execute(JobData data)
    {
        var membersIds = service.SelectGroup(data.GroupId, Guid.Empty)
            .DbMembers.Select(x => x.UserId);
    }
}