using System.Text.Json;

namespace Verbalize.Core
{
    /// <summary> The voice and settings the user had when they last closed the app; any of them can be missing. </summary>
    /// <param name="LanguageName">The readable name of the language, for example "Greek (Greece)".</param>
    /// <param name="VoiceName">The voice's display name, for example "Athina".</param>
    /// <param name="Style">The speaking style, for example "calm".</param>
    /// <param name="Rate">The speed, as a named speed or a percentage, for example "slow" or "10%".</param>
    /// <param name="Pitch">The pitch, as a named level or in hertz, for example "high" or "+5Hz".</param>
    /// <param name="Volume">The volume, from 0 to 100.</param>
    public sealed record UserPreferences(string? LanguageName, string? VoiceName, string? Style, string? Rate, string? Pitch, int? Volume)
    {
        /// <summary> No remembered settings, as on the first run. </summary>
        public static UserPreferences None { get; } = new(null, null, null, null, null, null);
    }

    /// <summary> Reads and writes the remembered voice and settings in a small file in the user's app data folder. </summary>
    public static class UserPreferencesStore
    {
        /// <summary> Gives where the remembered settings are kept, next to the Speech settings file. </summary>
        /// <param name="appDataFolder">The user's roaming app data folder.</param>
        /// <returns>The full path of the file, for example "%APPDATA%\Verbalize\preferences.json".</returns>
        public static string DefaultFilePath(string appDataFolder)
        {
            //  Put the file in the app's own folder.
            return Path.Combine(appDataFolder, "Verbalize", "preferences.json");
        }

        /// <summary> Reads the remembered settings, starting fresh if the file is missing or cannot be read. </summary>
        /// <param name="filePath">The file to read.</param>
        /// <returns>The remembered settings, or <see cref="UserPreferences.None"/>.</returns>
        public static UserPreferences Load(string filePath)
        {
            //  Start fresh when there is nothing remembered yet.
            if (!File.Exists(filePath)) { return UserPreferences.None; }

            //  Read the file, starting fresh if it is damaged or locked.
            try
            {
                return JsonSerializer.Deserialize<UserPreferences>(File.ReadAllText(filePath)) ?? UserPreferences.None;
            }
            catch (Exception exception) when (exception is JsonException or IOException or UnauthorizedAccessException or NotSupportedException)
            {
                return UserPreferences.None;
            }
        }

        /// <summary> Writes the settings to remember, creating the folder if needed. </summary>
        /// <param name="filePath">The file to write.</param>
        /// <param name="preferences">The settings to remember.</param>
        /// <returns>True if they were written; false if the file could not be written.</returns>
        public static bool Save(string filePath, UserPreferences preferences)
        {
            //  Write the file, giving up quietly if the folder cannot be written to.
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                File.WriteAllText(filePath, JsonSerializer.Serialize(preferences, new JsonSerializerOptions { WriteIndented = true }));
                return true;
            }
            catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
