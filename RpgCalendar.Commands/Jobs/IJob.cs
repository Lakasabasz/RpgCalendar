using RpgCalendar.Tools;

namespace RpgCalendar.Commands.Jobs;

public interface IJob
{
    public ErrorCode? Error { get; }
    
    public IApiResponse? ApiResponse { get; }
}