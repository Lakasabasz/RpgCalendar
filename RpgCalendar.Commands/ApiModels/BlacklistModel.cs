using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

record BlacklistModel(IEnumerable<UserShort> Users, IEnumerable<GroupShort> Groups): IApiResponse;