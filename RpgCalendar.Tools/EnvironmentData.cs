using System.Reflection;
using System.Runtime.CompilerServices;

namespace RpgCalendar.Tools;

public static class EnvironmentData
{
    private const string GraylogUrlEnv = "GRAYLOG_URL";
    public static string GraylogUrl => Environment.GetEnvironmentVariable(GraylogUrlEnv) ?? "localhost";

    private const string RelationalDbHostEnv = "API_RELATIONAL_DB_HOST";
    public static string RelationalDbHost => Environment.GetEnvironmentVariable(RelationalDbHostEnv) ?? "localhost";

    private const string RelationalDbPasswdEnv = "MYSQL_ROOT_PASSWORD";
    public static string RelationalDbPasswd => Environment.GetEnvironmentVariable(RelationalDbPasswdEnv) ?? "root";

    private const string KeycloakRealmEnv = "KEYCLOAK_REALM";
    public static string KeycloakRealm => Environment.GetEnvironmentVariable(KeycloakRealmEnv) ?? "rpgcalendar";
    
    private const string KeycloakInternalUrlEnv = "KEYCLOAK_INTERNAL_URL";
    public static string KeycloakInternalUrl => Environment.GetEnvironmentVariable(KeycloakInternalUrlEnv) ?? "http://localhost:8080";

    public static string KeycloakRealmUrl => $"{KeycloakInternalUrl}/realms/{KeycloakRealm}";
    public static string KeycloakMetadataUrl => $"{KeycloakRealmUrl}/.well-known/openid-configuration";
    
    private const string KeycloakAudienceEnv = "KEYCLOAK_AUDIENCE";
    public static string? KeycloakAudience => Environment.GetEnvironmentVariable(KeycloakAudienceEnv) ?? "account";

    private const string FeatureFlagPrefixEnv = "FF_";

    public static bool GetFlag(FeatureFlag.FeatureFlagEnum requiredFlag)
        => bool.TryParse(Environment.GetEnvironmentVariable(FeatureFlagPrefixEnv + requiredFlag), out var result) && result;
    
    private const string StaticFilesRootEnv = "FILESYSTEM_ROOT";
    public static string StaticFilesRoot 
        => Environment.GetEnvironmentVariable(StaticFilesRootEnv) 
        ?? Path.Join(Directory.GetParent(Assembly.GetExecutingAssembly().Location)?.ToString(), "files");
}