using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record EventsListModel(IEnumerable<EventShortModel> Events): IApiResponse;
public record EventShortModel(string Title, UserShortModel Creator, DateTime Start, DateTime End);