using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users;

public class GetUserDataJob: IJob
{
    public ErrorCode? Error { get; private set; }
    
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(User invoker)
    {
        ApiResponse = new UserModel(invoker.Nick, invoker.PrivateCode);
    }
}