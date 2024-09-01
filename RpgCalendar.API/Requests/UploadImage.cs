using System.ComponentModel.DataAnnotations;
using RpgCalendar.Tools.Enums;

namespace RpgCalendar.API.Requests;

public record UploadImage(ImageType TargetType, [MaxLength(1024*1024*2)] string Content);