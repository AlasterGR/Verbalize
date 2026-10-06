using System.Xml;

namespace Verbalize.Core
{
    /// <summary> The voices available for one language, and that language's code. </summary>
    /// <param name="DisplayNames">The display names of the voices, in list order, for example "Jenny".</param>
    /// <param name="Locale">The language code, for example "en-US", or null if no voice speaks that language.</param>
    public sealed record VoicesInLocale(IReadOnlyList<string> DisplayNames, string? Locale);

    /// <summary> Who a voice is: the name Azure knows it by, its name in its own language, and its gender. </summary>
    /// <param name="ShortName">The name Azure knows the voice by, for example "en-US-JennyNeural".</param>
    /// <param name="LocalName">The voice's name in its own language, for example "Αθηνά".</param>
    /// <param name="Gender">The voice's gender, for example "Female".</param>
    public sealed record VoiceIdentity(string ShortName, string LocalName, string Gender);

    /// <summary> Where a voice belongs in the app's lists: its language's name and its own display name. </summary>
    /// <param name="LocaleName">The name of the voice's language, for example "English (United States)".</param>
    /// <param name="DisplayName">The voice's display name, for example "Jenny".</param>
    public sealed record VoiceListing(string LocaleName, string DisplayName);

    /// <summary> Answers questions about a voice list, in the layout produced by <see cref="VoiceListConverter"/>. </summary>
    /// <remarks> Where several voices match, the last one wins, which matches the app's existing behaviour. </remarks>
    public static class VoiceCatalog
    {
        /// <summary> Lists each language once, in the order its first voice appears. </summary>
        /// <param name="voices">The voice list.</param>
        /// <returns>The language names, for example "Greek (Greece)", or an empty list if the voice list is empty.</returns>
        public static IReadOnlyList<string?> GetLocaleNames(XmlDocument voices)
        {
            //  Go through every voice, noting each language code the first time it appears.
            List<string?> localeNames = new();
            List<string?> uniqueLocales = new();
            foreach (XmlNode node in SelectVoices(voices))
            {
                string? locale = node.SelectSingleNode("Locale")?.InnerText;
                if (!uniqueLocales.Contains(locale))
                {
                    //  Remember the language's readable name the first time its code is seen.
                    uniqueLocales.Add(locale);
                    localeNames.Add(node.SelectSingleNode("LocaleName")?.InnerText);
                }
            }
            return localeNames;
        }

        /// <summary> Lists the voices that speak a given language. </summary>
        /// <param name="voices">The voice list.</param>
        /// <param name="localeName">The language name, for example "English (United States)".</param>
        /// <returns>The display names of the matching voices, and the language's code.</returns>
        public static VoicesInLocale GetVoicesInLocale(XmlDocument voices, string localeName)
        {
            //  Go through every voice, keeping the ones whose language name matches.
            List<string> displayNames = new();
            string? locale = null;
            foreach (XmlNode node in SelectVoices(voices))
            {
                string nodeLocaleName = node.SelectSingleNode("LocaleName")?.InnerText ?? string.Empty;
                if (nodeLocaleName == localeName)
                {
                    //  Note the voice's display name and its language code.
                    displayNames.Add(node.SelectSingleNode("DisplayName")!.InnerText);
                    locale = node.SelectSingleNode("Locale")!.InnerText;
                }
            }
            return new VoicesInLocale(displayNames, locale);
        }

        /// <summary> Finds a voice by its display name, optionally only among the voices of one language. </summary>
        /// <param name="voices">The voice list.</param>
        /// <param name="displayName">The display name, for example "Jenny".</param>
        /// <param name="localeName">The language to look in, for example "English (United States)", or null or empty to look in every language.</param>
        /// <returns>The voice's identity, or null if no voice has that display name.</returns>
        public static VoiceIdentity? FindVoiceByDisplayName(XmlDocument voices, string displayName, string? localeName = null)
        {
            //  Go through every voice, keeping the last one whose display name, and language if given, match.
            VoiceIdentity? match = null;
            foreach (XmlNode node in SelectVoices(voices))
            {
                string nodeDisplayName = node.SelectSingleNode("DisplayName")?.InnerText ?? string.Empty;
                if (nodeDisplayName == displayName && IsInLocale(node, localeName))
                {
                    match = new VoiceIdentity(
                        node.SelectSingleNode("ShortName")!.InnerText,
                        node.SelectSingleNode("LocalName")!.InnerText,
                        node.SelectSingleNode("Gender")!.InnerText);
                }
            }
            return match;
        }

        /// <summary> Finds a voice by the name Azure knows it by. </summary>
        /// <param name="voices">The voice list.</param>
        /// <param name="shortName">The voice's short name, for example "en-US-JennyNeural".</param>
        /// <returns>The voice's language name and display name, or null if no voice has that short name.</returns>
        public static VoiceListing? FindVoiceByShortName(XmlDocument voices, string shortName)
        {
            //  Go through every voice, keeping the last one whose short name matches.
            VoiceListing? match = null;
            foreach (XmlNode node in SelectVoices(voices))
            {
                if (node.SelectSingleNode("ShortName")!.InnerText == shortName)
                {
                    match = new VoiceListing(
                        node.SelectSingleNode("LocaleName")!.InnerText,
                        node.SelectSingleNode("DisplayName")!.InnerText);
                }
            }
            return match;
        }

        /// <summary> Lists the speaking styles, such as "cheerful" or "sad", that a voice supports. </summary>
        /// <param name="voices">The voice list.</param>
        /// <param name="displayName">The voice's display name, for example "Jenny".</param>
        /// <param name="localeName">The language to look in, for example "English (United States)", or null or empty to look in every language.</param>
        /// <returns>The style names, or an empty list if the voice is not found or has no styles.</returns>
        public static IReadOnlyList<string> GetStyles(XmlDocument voices, string? displayName, string? localeName = null)
        {
            //  Find the first voice with this display name, and language if given.
            List<string> styles = new();
            XmlNode? voiceNode = SelectVoices(voices).FirstOrDefault(node =>
                (node.SelectSingleNode("DisplayName")?.InnerText ?? string.Empty) == (displayName ?? string.Empty) && IsInLocale(node, localeName));

            //  Collect each of its styles, in list order.
            if (voiceNode != null)
            {
                foreach (XmlNode styleNode in voiceNode.SelectNodes("StyleList")!)
                {
                    styles.Add(styleNode.InnerText);
                }
            }
            return styles;
        }

        /// <summary> Checks whether a voice belongs to a language. </summary>
        /// <param name="voiceNode">The voice entry.</param>
        /// <param name="localeName">The language name, or null or empty to accept every language.</param>
        /// <returns>True if no language is given or the voice's language matches it.</returns>
        private static bool IsInLocale(XmlNode voiceNode, string? localeName)
        {
            //  Accept every voice when no language is given, otherwise only voices of that language.
            return string.IsNullOrEmpty(localeName) || voiceNode.SelectSingleNode("LocaleName")?.InnerText == localeName;
        }

        /// <summary> Returns every voice entry directly under the top of the voice list. </summary>
        /// <param name="voices">The voice list.</param>
        /// <returns>The voice entries, or none if the voice list is empty.</returns>
        private static IEnumerable<XmlNode> SelectVoices(XmlDocument voices)
        {
            //  Return nothing when the list has not been loaded, otherwise every "Voice" entry.
            XmlNodeList? nodes = voices.DocumentElement?.SelectNodes("Voice");
            return nodes == null ? Enumerable.Empty<XmlNode>() : nodes.Cast<XmlNode>();
        }
    }
}
