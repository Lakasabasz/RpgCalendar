using Microsoft.EntityFrameworkCore;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands;

public class EventService(RelationalDb db)
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

    public EventService SelectEvent(Guid eventId)
    {
        _eventId = eventId;
        return this;
    }

    public ErrorCode? AddEvent(Guid groupId, Guid creatorId, string title, string description, DateTime start, DateTime end,
        string? location, bool? isOnline)
    {
        var groupEvent = GroupEvent.Prepare(creatorId, groupId, title, description, start, end, location, isOnline);
        var otherUsersAbsences = db.PrivateEvents
            .Where(x => x.OwnerId != creatorId)
            .Where(x => (x.Start < start && start == x.End) || (x.Start < end && end < x.End)
                                || (start < x.Start && x.Start < end) || (start < x.End && x.End < end));
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
}