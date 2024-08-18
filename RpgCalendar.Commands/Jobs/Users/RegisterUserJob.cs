using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users;

public class RegisterUserJob(RelationalDb db): IJob
{
    public record JobData(Guid userId, string displayName);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        db.Users.Add(User.Prepare(data.userId, data.displayName, $"{Random.Shared.Next() % 1000000:D6}"));
        db.SaveChanges();
        var user = db.Users.First(x => x.Id == data.userId);
        ApiResponse = new UserModel(user.Nick, user.PrivateCode);
    }
}