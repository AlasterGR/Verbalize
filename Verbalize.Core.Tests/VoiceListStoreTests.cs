using System.Xml;
using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that the app uses the downloaded voice list when it can, and the built-in one otherwise. </summary>
    public sealed class VoiceListStoreTests : IDisposable
    {
        /// <summary> A temporary folder for the test files, removed after each test. </summary>
        private readonly string folder = Directory.CreateTempSubdirectory("verbalize-tests-").FullName;

        /// <summary> Removes the temporary folder and its files. </summary>
        public void Dispose()
        {
            //  Delete the folder and everything in it.
            Directory.Delete(folder, recursive: true);
        }

        /// <summary> Writes a voice list file with the given contents. </summary>
        /// <param name="contents">The file's contents.</param>
        /// <returns>The file's full path.</returns>
        private string WriteVoicesFile(string contents)
        {
            //  Save the contents in the temporary folder.
            string path = Path.Combine(folder, "Voices");
            File.WriteAllText(path, contents);
            return path;
        }

        /// <summary> A downloaded list with voices in it is used. </summary>
        [Fact]
        public void LoadOrDefault_UsesTheDownloadedList()
        {
            //  Load a saved list holding a single voice.
            string path = WriteVoicesFile("<?xml version=\"1.0\" encoding=\"utf-8\"?><Voices><Voice><DisplayName>Ava</DisplayName><Locale>en-US</Locale><LocaleName>English (United States)</LocaleName></Voice></Voices>");
            VoiceListLoadResult result = VoiceListStore.LoadOrDefault(path);

            //  Check the saved list was used.
            Assert.True(result.IsDownloadedList);
            Assert.Equal(new[] { "English (United States)" }, VoiceCatalog.GetLocaleNames(result.Voices));
        }

        /// <summary> With no downloaded list, the built-in list is used. </summary>
        [Fact]
        public void LoadOrDefault_FallsBackWhenThereIsNoFile()
        {
            //  Load from a file that does not exist.
            VoiceListLoadResult result = VoiceListStore.LoadOrDefault(Path.Combine(folder, "Voices"));

            //  Check the built-in list was used.
            Assert.False(result.IsDownloadedList);
            Assert.Equal(DefaultVoicesLocaleNames(), VoiceCatalog.GetLocaleNames(result.Voices));
        }

        /// <summary> A damaged downloaded list is ignored in favour of the built-in list. </summary>
        [Fact]
        public void LoadOrDefault_FallsBackWhenTheFileIsDamaged()
        {
            //  Load a file cut off half way through.
            VoiceListLoadResult result = VoiceListStore.LoadOrDefault(WriteVoicesFile("<Voices><Voice><DisplayName>Ava"));

            //  Check the built-in list was used.
            Assert.False(result.IsDownloadedList);
        }

        /// <summary> A downloaded list with no voices in it is ignored, so the app's lists are never left empty. </summary>
        [Fact]
        public void LoadOrDefault_FallsBackWhenTheFileHasNoVoices()
        {
            //  Load a list with no voices.
            VoiceListLoadResult result = VoiceListStore.LoadOrDefault(WriteVoicesFile("<Voices />"));

            //  Check the built-in list was used.
            Assert.False(result.IsDownloadedList);
        }

        /// <summary> Lists the languages in the built-in voice list. </summary>
        /// <returns>The built-in list's language names.</returns>
        private static IReadOnlyList<string?> DefaultVoicesLocaleNames()
        {
            //  Load the built-in list and list its languages.
            XmlDocument defaults = new();
            defaults.LoadXml(DefaultVoices.Xml);
            return VoiceCatalog.GetLocaleNames(defaults);
        }
    }
}
