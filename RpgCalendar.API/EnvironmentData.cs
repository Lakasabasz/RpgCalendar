﻿using System.Buffers.Text;

namespace RpgCalendar.API;

static class EnvironmentData
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

    private const string KeycloakAudienceEnv = "KEYCLOAK_AUDIENCE";
    public static string? KeycloakAudience => Environment.GetEnvironmentVariable(KeycloakAudienceEnv) ?? "account";
}