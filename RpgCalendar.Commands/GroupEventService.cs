using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands;

public class GroupEventService(RelationalDb db)
{
    private Guid? _eventId;
    private GroupEvent? _event;
    public GroupEvent DbEvent => _event ??= db.GroupEvents.First(x => x.GroupEventId == _eventId);
    
    private User? _creator;
    public User DbCreator => _creator ??= db.Users.First(x => x.Id == DbEvent.CreatorId);

    public IQueryable<EventUserRelationModel> UserRelationApiModels => db.UserGroupEventApprovals
        .Where(x => x.GroupEventId == _eventId)
        .Include(x => x.User)
        .Select(x => new EventUserRelationModel(new UserShortModel(x.User.Id, x.User.Nick), x.RelationTowardsEvent));
    
    private bool IsOverlappingEventExists(DateTime start, DateTime end)
    {
        var overlappingEvent = db.GroupEvents
            .Where(x => (x.StartTime < start && start == x.EndTime)
                        || (x.StartTime < end && end < x.EndTime)
                        || (start < x.StartTime && x.StartTime < end)
                        || (start < x.EndTime && x.EndTime < end))
            .FirstOrDefault(x => x.GroupEventId != DbEvent.GroupEventId);
        return overlappingEvent is not null;
    }

    public GroupEventService SelectEvent(Guid eventId)
    {
        _eventId = eventId;
        return this;
    }

    public ErrorCode? AddEvent(Guid groupId, Guid creatorId, string title, string description, DateTime start, DateTime end,
        string? location, bool? isOnline)
    {
        if (IsOverlappingEventExists(start, end)) return ErrorCode.CannotAddOverlappingEvent;
        
        var groupEvent = GroupEvent.Prepare(creatorId, groupId, title, description, start, end, location, isOnline);
        var otherUsersAbsences = db.PrivateEvents
            .Where(x => x.OwnerId != creatorId)
            .Where(x => (x.StartTime < start && start == x.EndTime) || (x.StartTime < end && end < x.EndTime)
                                || (start < x.StartTime && x.StartTime < end) || (start < x.EndTime && x.EndTime < end));
        db.GroupEvents.Add(groupEvent);
        db.UserGroupEventApprovals.AddRange(otherUsersAbsences.Select(x =>
            UserGroupEventApproval.Prepare(groupEvent.GroupEventId, x.OwnerId, RelationTowardsEventEnum.SoftReject)
            ));
        _eventId = groupEvent.GroupEventId;
        db.SaveChanges();
        return null;
    }
    
    public void Delete()
    {
        var relations = db.UserGroupEventApprovals.Where(x => x.GroupEventId == _eventId);
        db.UserGroupEventApprovals.RemoveRange(relations);
        db.GroupEvents.Remove(DbEvent);
        db.SaveChanges();
    }

    public IApiResponse? GetFullApiModel()
    {
        var creator = new UserShortModel(DbCreator.Id, DbCreator.Nick);
        return new EventFullModel(DbEvent.GroupEventId, DbEvent.Title, DbEvent.Description, creator, DbEvent.StartTime,
            DbEvent.EndTime, DbEvent.Summary ?? "", DbEvent.Location ?? "", DbEvent.IsOnline, UserRelationApiModels);
    }

    public ErrorCode? UpdateTitle(string title, bool save)
    {
        if (DbEvent.EndTime < DateTime.Now) return ErrorCode.CannotEditFinishedEvents;
        DbEvent.Title = title;
        if (save) db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateDescription(string description, bool save)
    {
        if (DbEvent.EndTime < DateTime.Now) return ErrorCode.CannotEditFinishedEvents;
        DbEvent.Description = description;
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateLocation(string location, bool save)
    {
        if (DbEvent.EndTime < DateTime.Now) return ErrorCode.CannotEditFinishedEvents;
        DbEvent.Location = location;
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateOnline(bool? isOnline, bool save)
    {
        if (DbEvent.EndTime < DateTime.Now) return ErrorCode.CannotEditFinishedEvents;
        DbEvent.IsOnline = isOnline;
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateSummary(string summary, bool save)
    {
        DbEvent.Summary = summary;
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? UpdateData(DateTime? start, DateTime? end, bool save)
    {
        if (DbEvent.EndTime < DateTime.Now) return ErrorCode.CannotEditFinishedEvents;
        var newStartTime = start ?? DbEvent.StartTime;
        var newEndTime = end ?? DbEvent.EndTime;
        if(newStartTime >= newEndTime) return ErrorCode.PatchInvalidTimeRange;

        if (IsOverlappingEventExists(newStartTime, newEndTime)) return ErrorCode.PatchOverlappingEvent;

        var currentRelations = db.UserGroupEventApprovals
            .Where(x => x.GroupEventId == _eventId)
            .ToList();
        
        var usersAbsences = db.PrivateEvents
            .Where(x => (x.StartTime < start && start == x.EndTime) || (x.StartTime < end && end < x.EndTime)
                                || (start < x.StartTime && x.StartTime < end) || (start < x.EndTime && x.EndTime < end))
            .ToList();
        
        List<UserGroupEventApproval> removeList = [];
        foreach (var currentRelation in currentRelations)
        {
            if(currentRelation.RelationTowardsEvent is RelationTowardsEventEnum.SoftReject)
            {
                if(usersAbsences.All(x => x.OwnerId != currentRelation.UserId))
                    removeList.Add(currentRelation);
            }
            else if(currentRelation.RelationTowardsEvent is RelationTowardsEventEnum.SoftRejectedAccept)
            {
                if (usersAbsences.All(x => x.OwnerId != currentRelation.UserId))
                    currentRelation.RelationTowardsEvent = RelationTowardsEventEnum.HardAccept;
            }
            else if(currentRelation.RelationTowardsEvent is RelationTowardsEventEnum.HardAccept)
            {
                if(usersAbsences.Any(x => x.OwnerId == currentRelation.UserId))
                    currentRelation.RelationTowardsEvent = RelationTowardsEventEnum.SoftRejectedAccept;
            }
        }
        
        db.UserGroupEventApprovals.RemoveRange(removeList);
        db.UserGroupEventApprovals.AddRange(usersAbsences
            .Where(x => currentRelations.Any(y => y.UserId == x.OwnerId))
            .Select(x => UserGroupEventApproval.Prepare(DbEvent.GroupEventId, x.OwnerId, RelationTowardsEventEnum.SoftReject))
            );
        
        if(save) db.SaveChanges();
        return null;
    }

    public ErrorCode? SetRelation(Guid invokerId, RelationTowardsEventEnum desiredRelation, bool save=true)
    {
        var relation = db.UserGroupEventApprovals.FirstOrDefault(x => x.GroupEventId == _eventId && x.UserId == invokerId);
        if(relation is not null)
        {
            if (relation.RelationTowardsEvent == desiredRelation)
                return ErrorCode.NoRelationChange;
            relation.RelationTowardsEvent = desiredRelation;
        }
        else{
            relation = UserGroupEventApproval.Prepare(_eventId ?? throw new ArgumentNullException(nameof(invokerId)), 
                invokerId, desiredRelation);
            db.UserGroupEventApprovals.Add(relation);
        }
        
        if(save) db.SaveChanges();
        return null;
    }
}