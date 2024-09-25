using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record UserShortModel(Guid userId, string displayName);
public record UserModel(Guid userId, string displayName, string privateCode, string profileImage, uint OwnedGroupLimit): IApiResponse;