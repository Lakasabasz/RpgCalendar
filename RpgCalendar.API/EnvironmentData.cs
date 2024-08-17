namespace RpgCalendar.API;

static class EnvironmentData
{
    private const string GraylogUrlEnv = "GRAYLOG_URL";
    public static string GraylogUrl => Environment.GetEnvironmentVariable(GraylogUrlEnv) ?? "localhost";
}