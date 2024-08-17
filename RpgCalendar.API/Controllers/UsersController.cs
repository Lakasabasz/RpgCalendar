﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace RpgCalendar.API.Controllers;

//[Authorize]
[ApiController, Route("/users")]
public class UsersController : Controller
{
    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok();
    }
    
    [HttpPost("login")]
    public IActionResult Login()
    {
        var secretKey = new SymmetricSecurityKey(EnvironmentData.JwtSigningKeyBytes);
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            claims: new List<Claim>()
            {
                new Claim("userid", Guid.NewGuid().ToString()),
            },
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signinCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return Ok(new{ Token = tokenString });
    }
}