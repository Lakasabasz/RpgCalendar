using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record BlacklistModel(IEnumerable<UserShortModel> Users, IEnumerable<GroupShort> Groups): IApiResponse;