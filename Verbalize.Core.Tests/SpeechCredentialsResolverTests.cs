using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks where the Azure key and region are taken from: environment variables first, then the settings file, then the default region. </summary>
    public sealed class SpeechCredentialsResolverTests : IDisposable
    {
        /// <summary> A temporary folder for the test files, removed after each test. </summary>
        private readonly string folder = Directory.CreateTempSubdirectory("verbalize-tests-").FullName;

        /// <summary> The settings file used by the tests; it only exists once a test writes it. </summary>
        private string SettingsFilePath => Path.Combine(folder, "settings.json");

        /// <summary> Removes the temporary folder and its files. </summary>
        public void Dispose()
        {
            //  Delete the folder and everything in it.
            Directory.Delete(folder, recursive: true);
        }

        /// <summary> Builds a stand-in for the computer's environment variables. </summary>
        /// <param name="key">The key variable's value, or null if it is not set.</param>
        /// <param name="region">The region variable's value, or null if it is not set.</param>
        /// <returns>A function that answers like Environment.GetEnvironmentVariable.</returns>
        private static Func<string, string?> Environment(string? key = null, string? region = null)
        {
            //  Answer with the given values for the two variables, and nothing for any other.
            return name => name switch
            {
                SpeechCredentialsResolver.KeyVariable => key,
                SpeechCredentialsResolver.RegionVariable => region,
                _ => null,
            };
        }

        /// <summary> Resolves the key and region with the given environment, the test settings file, and "westeurope" as the default region. </summary>
        /// <param name="environment">The stand-in environment variables.</param>
        /// <returns>The resolved key and region.</returns>
        private SpeechCredentials Resolve(Func<string, string?> environment)
        {
            //  Resolve using the test settings file.
            return SpeechCredentialsResolver.Resolve(environment, SettingsFilePath, "westeurope");
        }

        /// <summary> With nothing set anywhere, there is no key and the default region is used. </summary>
        [Fact]
        public void Resolve_HasNoKeyWhenNothingIsSet()
        {
            //  Resolve with no variables and no settings file.
            SpeechCredentials credentials = Resolve(Environment());

            //  Check there is no key, no problem, and the default region.
            Assert.False(credentials.HasKey);
            Assert.Null(credentials.Key);
            Assert.Equal("westeurope", credentials.Region);
            Assert.Null(credentials.Problem);
        }

        /// <summary> The environment variables are used when set. </summary>
        [Fact]
        public void Resolve_UsesTheEnvironmentVariables()
        {
            //  Resolve with both variables set, and check both are used.
            SpeechCredentials credentials = Resolve(Environment("env-key", "eastus"));
            Assert.Equal(new SpeechCredentials("env-key", "eastus", null), credentials);
            Assert.True(credentials.HasKey);
        }

        /// <summary> The settings file is used when the environment variables are not set. </summary>
        [Fact]
        public void Resolve_UsesTheSettingsFile()
        {
            //  Write a settings file and resolve with no variables set.
            File.WriteAllText(SettingsFilePath, """{ "SpeechKey": "file-key", "SpeechRegion": "northeurope" }""");

            //  Check the file's values are used.
            Assert.Equal(new SpeechCredentials("file-key", "northeurope", null), Resolve(Environment()));
        }

        /// <summary> Environment variables win over the settings file, one value at a time. </summary>
        [Fact]
        public void Resolve_PrefersEnvironmentVariablesOverTheSettingsFile()
        {
            //  Write a settings file, and set only the key variable.
            File.WriteAllText(SettingsFilePath, """{ "SpeechKey": "file-key", "SpeechRegion": "northeurope" }""");

            //  Check the key comes from the variable and the region from the file.
            Assert.Equal(new SpeechCredentials("env-key", "northeurope", null), Resolve(Environment(key: "env-key")));
        }

        /// <summary> Blank values count as not set, and spaces around values are removed. </summary>
        [Fact]
        public void Resolve_IgnoresBlankValuesAndTrimsSpaces()
        {
            //  Write a settings file with a padded key, and set the variables to blanks.
            File.WriteAllText(SettingsFilePath, """{ "SpeechKey": "  file-key  " }""");

            //  Check the blanks are skipped, the key is trimmed and the default region is used.
            Assert.Equal(new SpeechCredentials("file-key", "westeurope", null), Resolve(Environment("   ", "")));
        }

        /// <summary> The settings file's names are matched regardless of capitals. </summary>
        [Fact]
        public void Resolve_MatchesSettingNamesRegardlessOfCapitals()
        {
            //  Write a settings file with lower-case names, and check its values are still found.
            File.WriteAllText(SettingsFilePath, """{ "speechkey": "file-key", "SPEECHREGION": "uksouth" }""");
            Assert.Equal(new SpeechCredentials("file-key", "uksouth", null), Resolve(Environment()));
        }

        /// <summary> A damaged settings file is reported, and the environment variables are still used. </summary>
        [Fact]
        public void Resolve_ReportsADamagedSettingsFile()
        {
            //  Write a settings file that is not valid JSON, and set the key variable.
            File.WriteAllText(SettingsFilePath, """{ "SpeechKey": """);
            SpeechCredentials credentials = Resolve(Environment(key: "env-key"));

            //  Check the variable is used and the problem names the file.
            Assert.Equal("env-key", credentials.Key);
            Assert.NotNull(credentials.Problem);
            Assert.Contains(SettingsFilePath, credentials.Problem);
        }

        /// <summary> A settings file that is valid JSON but not a set of named values is reported. </summary>
        [Fact]
        public void Resolve_ReportsASettingsFileOfTheWrongShape()
        {
            //  Write a settings file holding a list instead of named values.
            File.WriteAllText(SettingsFilePath, """[ "file-key" ]""");
            SpeechCredentials credentials = Resolve(Environment());

            //  Check nothing was taken from it and the problem is reported.
            Assert.False(credentials.HasKey);
            Assert.NotNull(credentials.Problem);
        }

        /// <summary> The settings file lives in a "Verbalize" folder inside the given app data folder. </summary>
        [Fact]
        public void DefaultSettingsFilePath_IsInsideAVerbalizeFolder()
        {
            //  Build the path for a sample app data folder, and check it.
            Assert.Equal(Path.Combine("AppData", "Verbalize", "settings.json"), SpeechCredentialsResolver.DefaultSettingsFilePath("AppData"));
        }
    }
}
