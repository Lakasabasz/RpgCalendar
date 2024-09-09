using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record BlacklistModel(IEnumerable<UserShort> Users, IEnumerable<GroupShort> Groups): IApiResponse;