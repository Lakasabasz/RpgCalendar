using System.Buffers.Text;
using RpgCalendar.Commands.ApiModels;
using RpgCalendar.Tools;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.Commands.Jobs.Images;

public class SaveImageJob(ImageService imageService): IJob
{
    public record JobData(ImageType type, string base64Image);
    public ErrorCode? Error { get; }
    public IApiResponse? ApiResponse { get; private set; }

    public void Execute(JobData data)
    {
        Guid fileId = Guid.NewGuid();
        var bytes = Convert.FromBase64String(data.base64Image);
        
        imageService.Save(fileId, bytes);

        ApiResponse = new ImageCreation(fileId);
    }
}