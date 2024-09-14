using System.ComponentModel.DataAnnotations;

namespace RpgCalendar.API.Validation;

public class DateSpanAttribute: ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return base.IsValid(value);
    }
}