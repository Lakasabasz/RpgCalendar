using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpgCalendar.API.Requests;
using RpgCalendar.Commands.Jobs.Images;

namespace RpgCalendar.API.Controllers;

[Authorize]
[ApiController, Route("imgs")]
public class ImgController(Lazy<SaveImageJob> saveImageJob) : CustomController
{
    [HttpPost()]
    public IActionResult UploadBase64([FromBody] UploadImage payload)
    {
        saveImageJob.Value.Execute(new SaveImageJob.JobData(payload.TargetType, payload.Content));
        return HandleJobResult(saveImageJob.Value);
    }
}