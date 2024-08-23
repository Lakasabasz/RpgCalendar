﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.Commands;

namespace RpgCalendar.API.Controllers.Users;

[Authorize]
[ApiController, Route("/users/{userId:guid}")]
public class UserController(AccessTester tester) : CustomController
{
    [HttpPatch("limits")]
    public IActionResult UpdateLimits([FromQuery] int limit)
    {
        throw new NotImplementedException();
    }
}