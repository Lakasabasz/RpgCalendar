using RpgCalendar.Database;
using RpgCalendar.Database.Models;

namespace RpgCalendar.Commands;

public class EventService(RelationalDb db)
{
    private Guid? _eventId;
    private GroupEvent? _event;
    public GroupEvent DbEvent => _event ??= db.GroupEvents.First(x => x.GroupEventId == _eventId);

    public EventService SelectEvent(Guid eventId)
    {
        _eventId = eventId;
        return this;
    }
}