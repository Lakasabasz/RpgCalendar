using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;
using RpgCalendar.Tools.DbLinqExtensions;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class AddEventJob(RelationalDb db, GroupEventService eventService): IJob
{
    public record JobData(Guid OwnerId, string? Title, string? Description, DateOnly StartingDay, TimeOnly StartingHour,
        DateOnly EndingDay, TimeOnly EndingHour, bool IsOnline, string? Location, bool OverwriteApprovals);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        if (data.Title is null != data.Description is null)
        {
            Error = ErrorCode.TitleAndDescriptionMismatch;
            return;
        }
        var start = data.StartingDay.ToDateTime(data.StartingHour);
        var end = data.EndingDay.ToDateTime(data.EndingHour);
        var existsOverlappingEvent = db.PrivateEvents.Where(x => x.OwnerId == data.OwnerId)
            .WhereOverlapsTimeRange(start, end)
            .Any();
        if(existsOverlappingEvent)
        {
            Error = ErrorCode.CannotAddOverlappingEvent;
            return;
        }
        
        var groupsIds = db.GroupsMembers.Where(x => x.UserId == data.OwnerId).Select(x => x.GroupId).ToList();
        var overlappingEventsIds = db.GroupEvents
            .Where(x => groupsIds.Contains(x.GroupId))
            .WhereOverlapsTimeRange(start, end)
            .Select(x => x.GroupEventId);
        var userRelations = db.UserGroupEventApprovals
            .Where(x => x.UserId == data.OwnerId && overlappingEventsIds.Contains(x.GroupEventId))
            .ToList();
        foreach (var overlappingEventId in overlappingEventsIds)
        {
            eventService.SelectEvent(overlappingEventId);
            var relation = userRelations.SingleOrDefault(x => x.GroupEventId == overlappingEventId);
            if(relation is not null)
            {
                if(!data.OverwriteApprovals) continue;
                if(relation.RelationTowardsEvent is not RelationTowardsEventEnum.HardAccept) continue;
                eventService.SetRelation(data.OwnerId, RelationTowardsEventEnum.SoftRejectedAccept, false);
            }
            else
            {
                eventService.SetRelation(data.OwnerId, RelationTowardsEventEnum.SoftReject, false);
            }
        }
        
        var model = Database.Models.PrivateEvent.Prepare(data.Title, data.Description,
            data.Title is null,
            new DateTime(data.StartingDay, data.StartingHour), new DateTime(data.EndingDay, data.EndingHour),
            data.OwnerId, data.IsOnline, data.Location);
        db.PrivateEvents.Add(model);
        db.SaveChanges();
        var saved = db.PrivateEvents.First(x => x.EventId == model.EventId);
        
        ApiResponse = FullPrivateEvent.FromDateTime(saved.EventId, saved.OwnerId,
            saved.Title, saved.Description, saved.StartTime, saved.EndTime, saved.IsOnline, saved.Location);
    }
}