using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using RpgCalendar.Tools;

namespace RpgCalendar.API;

class ExceptionFallback(ILogger<ExceptionFallback> logger): IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception during request handling");
        
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.WriteAsJsonAsync(new ErrorApiModel(ErrorCode.SomethingWentWrong, "Unhandled exception occurred"),
            cancellationToken: cancellationToken);
        
        return ValueTask.FromResult(true);
    }
}