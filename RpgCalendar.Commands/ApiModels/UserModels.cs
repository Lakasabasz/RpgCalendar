using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

public record UserShortModel(Guid userId, string displayName);
public record UserShortWithProfileModel(Guid userId, string displayName, string profileImage);
public record UserModel(Guid userId, string displayName, string privateCode, string profileImage, uint OwnedGroupLimit): IApiResponse;