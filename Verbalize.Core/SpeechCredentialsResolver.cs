using System.Text.Json;

namespace Verbalize.Core
{
    /// <summary> The Azure Speech key and region the app should use. </summary>
    /// <param name="Key">The subscription key, or null if none is set.</param>
    /// <param name="Region">The Azure region, for example "westeurope".</param>
    /// <param name="Problem">A description of a problem with the settings file, or null if there was none.</param>
    public sealed record SpeechCredentials(string? Key, string Region, string? Problem)
    {
        /// <summary> True if a key is set. </summary>
        public bool HasKey => !string.IsNullOrWhiteSpace(Key);
    }

    /// <summary> Finds the Azure Speech key and region, which are kept outside the code so they are never shared with it. </summary>
    public static class SpeechCredentialsResolver
    {
        /// <summary> The environment variable that holds the subscription key. </summary>
        public const string KeyVariable = "VERBALIZE_SPEECH_KEY";
        /// <summary> The environment variable that holds the Azure region. </summary>
        public const string RegionVariable = "VERBALIZE_SPEECH_REGION";
        /// <summary> The settings file's name for the subscription key. </summary>
        public const string KeySetting = "SpeechKey";
        /// <summary> The settings file's name for the Azure region. </summary>
        public const string RegionSetting = "SpeechRegion";

        /// <summary> Returns where the settings file is kept: a "Verbalize" folder inside the user's app data folder. </summary>
        /// <param name="appDataFolder">The user's app data folder, for example %APPDATA% on Windows.</param>
        /// <returns>The settings file's full path.</returns>
        public static string DefaultSettingsFilePath(string appDataFolder)
        {
            //  Put the file in the app's own folder.
            return Path.Combine(appDataFolder, "Verbalize", "settings.json");
        }

        /// <summary> Finds the key and region, taking each from its environment variable if set, otherwise from the settings file, with the region falling back to a default. </summary>
        /// <param name="getEnvironmentVariable">Reads an environment variable, normally Environment.GetEnvironmentVariable.</param>
        /// <param name="settingsFilePath">Where the settings file is kept; it does not have to exist.</param>
        /// <param name="defaultRegion">The region to use when none is set.</param>
        /// <returns>The key and region, and any problem found with the settings file.</returns>
        public static SpeechCredentials Resolve(Func<string, string?> getEnvironmentVariable, string settingsFilePath, string defaultRegion)
        {
            //  Read the key and region from the settings file, if there is one.
            string? fileKey = null, fileRegion = null, problem = null;
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    using JsonDocument settings = JsonDocument.Parse(File.ReadAllText(settingsFilePath));
                    if (settings.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        fileKey = ReadSetting(settings.RootElement, KeySetting);
                        fileRegion = ReadSetting(settings.RootElement, RegionSetting);
                    }
                    else
                    {
                        problem = $"The settings file {settingsFilePath} must contain named values, like {{ \"{KeySetting}\": \"...\" }}.";
                    }
                }
                //  Note a file that cannot be read, so the user can be told about it.
                catch (Exception exception) when (exception is JsonException or IOException or UnauthorizedAccessException)
                {
                    problem = $"The settings file {settingsFilePath} could not be read: {exception.Message}";
                }
            }

            //  Take each value from its environment variable first, then the file, then the default region.
            string? key = FirstNonBlank(getEnvironmentVariable(KeyVariable), fileKey);
            string region = FirstNonBlank(getEnvironmentVariable(RegionVariable), fileRegion) ?? defaultRegion;
            return new SpeechCredentials(key, region, problem);
        }

        /// <summary> Reads a text value from the settings, matching its name regardless of capitals. </summary>
        /// <param name="settings">The settings' named values.</param>
        /// <param name="name">The name to look for.</param>
        /// <returns>The value, or null if it is missing or not text.</returns>
        private static string? ReadSetting(JsonElement settings, string name)
        {
            //  Return the first value with this name that holds text.
            foreach (JsonProperty property in settings.EnumerateObject())
            {
                if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase) && property.Value.ValueKind == JsonValueKind.String)
                {
                    return property.Value.GetString();
                }
            }
            return null;
        }

        /// <summary> Returns the first value that is not blank, without spaces around it. </summary>
        /// <param name="values">The values to try, in order of preference.</param>
        /// <returns>The first non-blank value, trimmed, or null if all are blank.</returns>
        private static string? FirstNonBlank(params string?[] values)
        {
            //  Skip blank values and trim the first real one.
            return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))?.Trim();
        }
    }
}
