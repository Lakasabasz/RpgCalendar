using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RpgCalendar.Commands.Jobs.Users;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers;

//[Authorize]
[ApiController, Route("/users")]
public class UsersController(GetUserDataJob getUserDataJob) : CustomController
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        getUserDataJob.Execute(Invoker);
        return HandleJobResult(getUserDataJob);
    }
    
    [HttpPost("login")]
    public IActionResult Login()
    {
        var secretKey = new SymmetricSecurityKey(EnvironmentData.JwtSigningKeyBytes);
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            claims: new List<Claim>()
            {
                new Claim(Consts.JwtConsts.UserId, Guid.NewGuid().ToString()),
            },
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return Ok(new{ Token = tokenString });
    }
}