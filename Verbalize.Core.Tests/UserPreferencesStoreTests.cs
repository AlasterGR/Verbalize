using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that the remembered voice and settings are kept between runs. </summary>
    public sealed class UserPreferencesStoreTests : IDisposable
    {
        /// <summary> A temporary folder for the test files, removed after each test. </summary>
        private readonly string folder = Directory.CreateTempSubdirectory("verbalize-tests-").FullName;

        /// <summary> Removes the temporary folder and its files. </summary>
        public void Dispose()
        {
            //  Delete the folder and everything in it.
            Directory.Delete(folder, recursive: true);
        }

        /// <summary> The file sits in the app's own folder. </summary>
        [Fact]
        public void DefaultFilePath_IsInTheAppsFolder()
        {
            //  Work out the path for a sample app data folder.
            string path = UserPreferencesStore.DefaultFilePath(folder);

            //  Check it is the preferences file in the Verbalize folder.
            Assert.Equal(Path.Combine(folder, "Verbalize", "preferences.json"), path);
        }

        /// <summary> Saved settings come back the same, and the folder is created if needed. </summary>
        [Fact]
        public void SaveThenLoad_GivesBackTheSameSettings()
        {
            //  Save some settings into a folder that does not exist yet.
            string path = UserPreferencesStore.DefaultFilePath(folder);
            UserPreferences saved = new("Greek (Greece)", "Athina", "calm", "10%", "+5Hz", 60);
            bool written = UserPreferencesStore.Save(path, saved);

            //  Check they were written and read back unchanged.
            Assert.True(written);
            Assert.Equal(saved, UserPreferencesStore.Load(path));
        }

        /// <summary> With no file yet, nothing is remembered. </summary>
        [Fact]
        public void Load_WithNoFile_RemembersNothing()
        {
            //  Load from a file that does not exist.
            UserPreferences loaded = UserPreferencesStore.Load(Path.Combine(folder, "missing.json"));

            //  Check nothing is remembered.
            Assert.Equal(UserPreferences.None, loaded);
        }

        /// <summary> A damaged file is ignored rather than stopping the app. </summary>
        [Fact]
        public void Load_WithADamagedFile_RemembersNothing()
        {
            //  Write a file that is not valid JSON.
            string path = Path.Combine(folder, "preferences.json");
            File.WriteAllText(path, "{ not json");

            //  Check nothing is remembered.
            Assert.Equal(UserPreferences.None, UserPreferencesStore.Load(path));
        }
    }
}
