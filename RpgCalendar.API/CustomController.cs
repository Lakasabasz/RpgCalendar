﻿using Microsoft.AspNetCore.Mvc;
using RpgCalendar.Commands.Jobs;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.API;

public class CustomController: Controller
{
    public User? Invoker => HttpContext.Items[Consts.AuthConsts.UserContextField] as User;

    protected IActionResult HandleJobResult(IJob job)
    {
        if (job.ApiResponse is not null) return Ok(job.ApiResponse);
        return job.Error is not null ? LateError(job.Error.Value) : Ok();
    }
    
    protected IActionResult EarlyError(ErrorCode errorCode)
    {
        return StatusCode(StatusCodes.Status418ImATeapot,
            Consts.Errors.ErrorCodeMessages.TryGetValue(errorCode, out var message)
                ? message
                : Consts.Errors.FallbackErrorMessage(errorCode));
    }

    protected IActionResult LateError(ErrorCode errorCode)
    {
        return StatusCode(420, Consts.Errors.ErrorCodeMessages.TryGetValue(errorCode, out var message)
            ? message
            : Consts.Errors.FallbackErrorMessage(errorCode));
    }
}