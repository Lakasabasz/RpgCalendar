using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

record UserShort(Guid userId, string displayName);
record UserModel(string displayName, string privateCode, string profileImage, uint OwnedGroupLimit): IApiResponse;