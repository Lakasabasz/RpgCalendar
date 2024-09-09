using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Groups;

public class JoinGroupJob(RelationalDb db, ImageService imageService, GroupService groupService): IJob
{
    public record JobData(Guid InvokerId, Guid InviteId);

    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        Error = groupService.Join(out var groupDto, data.InviteId, data.InvokerId);
        
        ApiResponse = groupDto?.ToFullGroup();
    }
}