using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;
using RpgCalendar.Tools.DbLinqExtensions;
using RpgCalendar.Tools.Enums;
using PrivateEvent = RpgCalendar.Database.Models.PrivateEvent;

namespace RpgCalendar.Commands;

public class PrivateEventService(RelationalDb db, Lazy<GroupEventService> groupEventServiceLazy)
{
    private Guid? _eventId;
    private PrivateEvent? _event;
    
    public PrivateEvent Event => _event ??= db.PrivateEvents.First(x => x.EventId == _eventId);
    public ErrorCode? AddEvent(string? title, string? description, DateTime start, DateTime end, Guid userId, bool overwriteApprovals, bool isOnline, string? location)
    {
        if (title is null != description is null) return ErrorCode.TitleAndDescriptionMismatch;
        var existsOverlappingEvent = db.PrivateEvents.Where(x => x.OwnerId == userId)
            .WhereOverlapsTimeRange(start, end)
            .Any();
        if(existsOverlappingEvent) return ErrorCode.CannotAddOverlappingEvent;
        
        var groupsIds = db.GroupsMembers.Where(x => x.UserId == userId).Select(x => x.GroupId).ToList();
        var overlappingEventsIds = db.GroupEvents
            .Where(x => groupsIds.Contains(x.GroupId))
            .WhereOverlapsTimeRange(start, end)
            .Select(x => x.GroupEventId);
        var userRelations = db.UserGroupEventApprovals
            .Where(x => x.UserId == userId && overlappingEventsIds.Contains(x.GroupEventId))
            .ToList();
        foreach (var overlappingEventId in overlappingEventsIds)
        {
            var groupEventService = groupEventServiceLazy.Value;
            groupEventService.SelectEvent(overlappingEventId);
            var relation = userRelations.SingleOrDefault(x => x.GroupEventId == overlappingEventId);
            if(relation is not null)
            {
                if(!overwriteApprovals) continue;
                if(relation.RelationTowardsEvent is not RelationTowardsEventEnum.HardAccept) continue;
                groupEventService.SetRelation(userId, RelationTowardsEventEnum.SoftRejectedAccept, false);
            }
            else
            {
                groupEventService.SetRelation(userId, RelationTowardsEventEnum.SoftReject, false);
            }
        }
        
        var model = Database.Models.PrivateEvent.Prepare(title, description, title is null,
            start, end, userId, isOnline, location);
        db.PrivateEvents.Add(model);
        db.SaveChanges();
        _eventId = model.EventId;
        return null;
    }
    
    public FullPrivateEventModel GetFullApiModel()
    {
        return FullPrivateEventModel.FromDateTime(Event.EventId, Event.OwnerId,
            Event.Title, Event.Description, Event.StartTime, Event.EndTime, Event.IsOnline, Event.Location);
    }
}