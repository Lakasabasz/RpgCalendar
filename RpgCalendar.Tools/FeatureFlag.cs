namespace RpgCalendar.Tools;

public static class FeatureFlag
{
    public enum FeatureFlagEnum
    {
        KEYCLOAK_CERT
    }
    
    public static void RequireFeatureFlag(FeatureFlagEnum requiredFlag, Action action)
    {
        if (EnvironmentData.GetFlag(requiredFlag)) action();
    }
    
    public static T? RequireFeatureFlag<T>(FeatureFlagEnum requiredFlag, Func<T> action, T? defaultValue = default)
        => EnvironmentData.GetFlag(requiredFlag) ? action() : defaultValue;
}