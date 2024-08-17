namespace RpgCalendar.API;

static class EnvironmentData
{
    private const string GraylogUrlEnv = "GRAYLOG_URL";
    public static string GraylogUrl => Environment.GetEnvironmentVariable(GraylogUrlEnv) ?? "localhost";

    private const string RelationalDbHostEnv = "API_RELATIONAL_DB_HOST";
    public static string RelationalDbHost => Environment.GetEnvironmentVariable(RelationalDbHostEnv) ?? "localhost";

    private const string RelationalDbPasswdEnv = "MYSQL_ROOT_PASSWORD";
    public static string RelationalDbPasswd => Environment.GetEnvironmentVariable(RelationalDbPasswdEnv) ?? "root";

    private const string JwtSigningKeyEnv = "JWT_SECRET";
    public static string JwtSigningKey =>
        Environment.GetEnvironmentVariable(JwtSigningKeyEnv) ?? "my-super-secret-key-123456789";
}