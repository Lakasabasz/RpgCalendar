using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RpgCalendar.Database;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Middlewares;

class InjectUserMiddleware(RequestDelegate next, RelationalDb db)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        var userId = ctx.User.FindFirstValue(Consts.JwtConsts.UserId);
        var parseSuccess = Guid.TryParse(userId, out var userGuid);
        ctx.Items[Consts.AuthConsts.UserContextField] = parseSuccess 
            ? await db.Users.FirstOrDefaultAsync(x => x.Id == userGuid)
            : null;
        await next(ctx);
    }
}

public static class InjectUserMiddlewareExtensions
{
    public static IApplicationBuilder UseUserInjection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<InjectUserMiddleware>();
    }
}