using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Database;
using RpgCalendar.Tools;
using RpgCalendar.Tools.DbLinqExtensions;

namespace RpgCalendar.Commands.Jobs.Groups.Events;

public class GetAbsencesListJob(GroupService service, RelationalDb db, ImageService imageService): IJob
{
    public record JobData(Guid GroupId, DateTime StartDate, DateTime EndDate);
    public ErrorCode? Error => null;
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        var members = service.SelectGroup(data.GroupId, Guid.Empty).DbMembers
            .Select(x => x.User).ToList();
        var membersIds = members.Select(x => x.Id);
        
        var absences = db.PrivateEvents
            .Where(x => membersIds.Contains(x.OwnerId))
            .WhereOverlapsTimeRange(data.StartDate, data.EndDate)
            .ToList();
        
        List<GroupRequestedAbsenceModel> grAbsences = [];
        foreach (var absencePeriods in absences.GroupBy(x => x.OwnerId))
        {
            var ownerModel = members.First(x => x.Id == absencePeriods.Key);
            var owner = new UserShortWithProfileModel(
                ownerModel.Id, ownerModel.Nick, imageService.GetImageUrl(ownerModel.ProfilePicture));
            var absencesModel = new GroupRequestedAbsenceModel(
                absencePeriods.Select(x => new AbsencePeriodSimpleModel(x.EventId, x.StartTime, x.EndTime)),
                owner);
            grAbsences.Add(absencesModel);
        }

        ApiResponse = new GroupRequestedAbsencesModel(grAbsences);
    }
}