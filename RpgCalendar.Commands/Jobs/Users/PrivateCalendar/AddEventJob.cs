using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs.Users.PrivateCalendar;

public class AddEventJob(RelationalDb db): IJob
{
    public record JobData(Guid OwnerId, string? Title, string? Description, DateOnly StartingDay, TimeOnly StartingHour,
        DateOnly EndingDay, TimeOnly EndingHour);
    public ErrorCode? Error { get; private set; }
    public IApiResponse? ApiResponse { get; private set; }

    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
    // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataQuery
    public void Execute(JobData data)
    {
        if (data.Title is null != data.Description is null)
        {
            Error = ErrorCode.TitleAndDescriptionMismatch;
            return;
        }
        var model = Database.Models.PrivateEvent.Prepare(data.Title, data.Description,
            data.Title is null,
            new DateTime(data.StartingDay, data.StartingHour), new DateTime(data.EndingDay, data.EndingHour),
            data.OwnerId);
        db.PrivateEvents.Add(model);
        db.SaveChanges();
        var saved = db.PrivateEvents.First(x => x.EventId == model.EventId);
        ApiResponse = FullPrivateEvent.FromDateTime(saved.EventId, saved.OwnerId,
            saved.Title, saved.Description, saved.Start, saved.End);
    }
}