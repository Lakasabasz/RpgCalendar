namespace RpgCalendar.Tools;

public record ErrorApiModel(int errorCode, ErrorCode error, string message)
{
    public ErrorApiModel(ErrorCode error, string message) : this((int)error, error, message){}
}