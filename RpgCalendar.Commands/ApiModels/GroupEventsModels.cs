using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands.ApiModels;

public record EventsListModel(IEnumerable<EventShortModel> Events): IApiResponse;
public record EventShortModel(Guid EventId, string Title, UserShortModel Creator, DateTime Start, DateTime End);

public record EventFullModel(Guid EventId, string Title, string Description, UserShortModel Creator, DateTime Start, DateTime End,
    string Summary, string Location, bool? IsOnline, IEnumerable<EventUserRelationModel> Relations): IApiResponse;
public record EventUserRelationModel(UserShortModel User, RelationTowardsEventEnum Relation);