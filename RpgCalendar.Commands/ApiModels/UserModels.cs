using RpgCalendar.Tools;

namespace RpgCalendar.Commands.ApiModels;

record UserModel(string displayName, string privateCode, string profileImage): IApiResponse;