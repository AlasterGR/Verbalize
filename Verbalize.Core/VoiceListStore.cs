using System.Xml;

namespace Verbalize.Core
{
    /// <summary> The voice list to use, and whether it is the one downloaded from Azure. </summary>
    /// <param name="Voices">The voice list.</param>
    /// <param name="IsDownloadedList">True if it is the downloaded list, false if it is the built-in one.</param>
    public sealed record VoiceListLoadResult(XmlDocument Voices, bool IsDownloadedList);

    /// <summary> Chooses which voice list the app works with: the one downloaded from Azure, or the built-in one. </summary>
    public static class VoiceListStore
    {
        /// <summary> Loads the downloaded voice list, or the built-in one if the download is missing, unreadable or empty. </summary>
        /// <param name="path">Where the downloaded voice list is saved.</param>
        /// <returns>The voice list to use.</returns>
        public static VoiceListLoadResult LoadOrDefault(string path)
        {
            //  Use the downloaded list if it exists, reads correctly and has at least one voice.
            if (File.Exists(path))
            {
                try
                {
                    XmlDocument downloaded = new();
                    downloaded.Load(path);
                    if (downloaded.DocumentElement?.SelectNodes("Voice")?.Count > 0)
                    {
                        return new VoiceListLoadResult(downloaded, true);
                    }
                }
                catch (Exception exception) when (exception is XmlException or IOException or UnauthorizedAccessException)
                {
                    //  Ignore a damaged or locked file and use the built-in list below.
                }
            }

            //  Otherwise use the built-in list.
            XmlDocument builtIn = new();
            builtIn.LoadXml(DefaultVoices.Xml);
            return new VoiceListLoadResult(builtIn, false);
        }
    }
}
