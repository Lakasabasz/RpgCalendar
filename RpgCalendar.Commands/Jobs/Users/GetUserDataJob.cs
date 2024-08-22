using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users;

public class GetUserDataJob(RelationalDb db, ImageService imageService): IJob
{
    public ErrorCode? Error { get; private set; }
    
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(User invoker)
    {
        var user = db.Users.First(x => x.Id == invoker.Id);
        ApiResponse = new UserModel(invoker.Nick, invoker.PrivateCode, imageService.GetImageUrl(user.ProfilePicture));
    }
}