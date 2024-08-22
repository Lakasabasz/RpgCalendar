using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RpgCalendar.Commands.Jobs.Users;
using RpgCalendar.Tools;

namespace RpgCalendar.API.Controllers;

public record UserModel([MaxLength(64), MinLength(3)] string displayName, Guid? ProfilePicture);

//[Authorize]
[ApiController, Route("/users")]
public class UsersController(Lazy<GetUserDataJob> getUserDataJob, Lazy<RegisterUserJob> registerUserJob) : CustomController
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        if (Invoker is null) return EarlyError(ErrorCode.UserNotRegistered);
        getUserDataJob.Value.Execute(Invoker);
        return HandleJobResult(getUserDataJob.Value);
    }

    [Authorize]
    [HttpPost("")]
    public IActionResult Register([FromBody] UserModel user)
    {
        if (Invoker is not null) return EarlyError(ErrorCode.UserAlreadyRegistered);
        registerUserJob.Value.Execute(new RegisterUserJob.JobData(InvokerGuid, user.displayName, user.ProfilePicture));
        return HandleJobResult(registerUserJob.Value);
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