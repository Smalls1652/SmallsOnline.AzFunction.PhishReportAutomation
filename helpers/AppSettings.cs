namespace SmallsOnline.AzFunctions.PhishReportAutomation.Helpers;

/// <summary>
/// Helper class to get app settings stored in environment variables.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Get the value of an app setting.
    /// </summary>
    /// <param name="settingName">The name of the setting (Environment variable).</param>
    /// <returns>The value of the specified setting.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetSettingValue(string settingName)
    {
        string? value = Environment.GetEnvironmentVariable(
            variable: settingName,
            target: EnvironmentVariableTarget.Process
        );

        if (value is null)
        {
            throw new ArgumentNullException($"The value of the setting '{settingName}' is null.");
        }

        return value;
    }
}