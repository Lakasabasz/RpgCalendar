using Microsoft.AspNetCore.Mvc;
using RpgCalendar.Database.Models;
using RpgCalendar.Tools;

namespace RpgCalendar.API;

public class CustomController: Controller
{
    public User? Invoker => HttpContext.Items[Consts.AuthConsts.UserContextField] as User;
}