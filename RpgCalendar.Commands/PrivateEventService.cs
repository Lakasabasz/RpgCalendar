using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
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
    
    public IQueryable<PrivateEvent> UserPrivateEvents => db.PrivateEvents.Where(x => x.OwnerId == Event.OwnerId);

    private IQueryable<GroupEvent> _getOverlappingGroupEventsIds(Guid userId, DateTime start, DateTime end)
    {
        var groupsIds = db.GroupsMembers.Where(x => x.UserId == userId).Select(x => x.GroupId).ToList();
        return db.GroupEvents
            .Where(x => groupsIds.Contains(x.GroupId))
            .WhereOverlapsTimeRange(start, end);
    }
    
    public ErrorCode? AddEvent(string? title, string? description, DateTime start, DateTime end, Guid userId, bool overwriteApprovals, bool isOnline, string? location)
    {
        if (title is null != description is null) return ErrorCode.TitleAndDescriptionMismatch;

        var overlappingEventsIds = _getOverlappingGroupEventsIds(userId, start, end).Select(x => x.GroupEventId);
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
        
        var model = PrivateEvent.Prepare(title, description, title is null,
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
    
    public void SelectEvent(Guid eventId) => _eventId = eventId;
    
    public ErrorCode? UpdateTitle(string newTitle, bool save = true)
    {
        if(Event.SimpleAbsence) return ErrorCode.CannotChangeEventType;
        Event.Title = newTitle;
        if(save) db.SaveChanges();
        return null;
    }
    
    public ErrorCode? UpdateDescription(string newDescription, bool save = true)
    {
        if(Event.SimpleAbsence) return ErrorCode.CannotChangeEventType;
        Event.Description = newDescription;
        if(save) db.SaveChanges();
        return null;
    }
    
    public ErrorCode? UpdateDateTime(DateTime newStart, DateTime newEnd, bool overwriteApprovals, bool save = true)
    {
        if(newStart >= newEnd) return ErrorCode.InvalidTimeRange;
        var overlappingEvents = _getOverlappingGroupEventsIds(Event.OwnerId, Event.StartTime, Event.EndTime);
        var singleOverlappedEvents = overlappingEvents
            .Where(x => UserPrivateEvents.WhereOverlapsTimeRange(Event.StartTime, Event.EndTime).Count() == 1);
        var releasedEvents = singleOverlappedEvents.WhereNotOverlapsTimeRange(newStart, newEnd);
        
        foreach (var releasedEvent in releasedEvents)
        {
            var groupEventService = groupEventServiceLazy.Value;
            groupEventService.SelectEvent(releasedEvent.GroupEventId);
            var relation = groupEventService.UserRelations.First(x => x.UserId == Event.OwnerId);
            if(relation.RelationTowardsEvent is RelationTowardsEventEnum.SoftReject)
                db.UserGroupEventApprovals.Remove(relation);
            else if(relation.RelationTowardsEvent is RelationTowardsEventEnum.SoftRejectedAccept) 
                relation.RelationTowardsEvent = RelationTowardsEventEnum.HardAccept;
        }
        
        var newOverlappedEvents = _getOverlappingGroupEventsIds(Event.OwnerId, newStart, newEnd)
            .Where(newEvent => !overlappingEvents.Any(existing => existing.GroupEventId == newEvent.GroupEventId));
        foreach (var newOverlappedEvent in newOverlappedEvents)
        {
            var groupEventService = groupEventServiceLazy.Value;
            groupEventService.SelectEvent(newOverlappedEvent.GroupEventId);
            var relation = groupEventService.UserRelations.FirstOrDefault(x => x.UserId == Event.OwnerId);
            
            ErrorCode? errorCode = null;
            if(relation?.RelationTowardsEvent is RelationTowardsEventEnum.HardAccept && overwriteApprovals)
                errorCode = groupEventService.SetRelation(Event.OwnerId, RelationTowardsEventEnum.SoftRejectedAccept, false);
            else if(relation is null) 
                errorCode = groupEventService.SetRelation(Event.OwnerId, RelationTowardsEventEnum.SoftReject, false);
            
            if(errorCode is not null) return errorCode;
        }
        
        Event.StartTime = newStart;
        Event.EndTime = newEnd;
        
        if(save) db.SaveChanges();
        return null;
    }
    
    public ErrorCode? UpdateLocation(string newLocation, bool save = true)
    {
        Event.Location = newLocation;
        if(save) db.SaveChanges();
        return null;
    }
    
    public ErrorCode? UpdateIsOnline(bool? newOnline, bool save = true)
    {
        Event.IsOnline = newOnline ?? false;
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? Delete()
    {
        var releasedEvents = _getOverlappingGroupEventsIds(Event.OwnerId, Event.StartTime, Event.EndTime)
            .Where(x => UserPrivateEvents.WhereOverlapsTimeRange(Event.StartTime, Event.EndTime).Count() == 1);
        
        foreach (var releasedEvent in releasedEvents)
        {
            var groupEventService = groupEventServiceLazy.Value;
            groupEventService.SelectEvent(releasedEvent.GroupEventId);
            var relation = groupEventService.UserRelations.First(x => x.UserId == Event.OwnerId);
            if(relation.RelationTowardsEvent is RelationTowardsEventEnum.SoftReject)
                db.UserGroupEventApprovals.Remove(relation);
            else if(relation.RelationTowardsEvent is RelationTowardsEventEnum.SoftRejectedAccept) 
                relation.RelationTowardsEvent = RelationTowardsEventEnum.HardAccept;
        }
        
        db.PrivateEvents.Remove(Event);
        db.SaveChanges();
        return null;
    }
}