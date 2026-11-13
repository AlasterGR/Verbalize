namespace Verbalize.Core
{
    /// <summary> A language in the voice list: its code and its readable name. </summary>
    /// <param name="Locale">The language code, for example "el-GR".</param>
    /// <param name="LocaleName">The readable name, for example "Greek (Greece)".</param>
    public sealed record VoiceLanguage(string Locale, string LocaleName);

    /// <summary> Decides which language the language list starts at, so the user does not have to scroll from the top. </summary>
    public static class StartLanguage
    {
        /// <summary> Picks the language to start at: the one the user had before, else the user's own language and region, else any region of the user's language. </summary>
        /// <param name="languages">The languages in the list.</param>
        /// <param name="rememberedLocaleName">The readable name of the language the user had before, or null if there is none.</param>
        /// <param name="userCulture">The user's language and region from Windows, for example "el-GR".</param>
        /// <returns>The readable name of the language to start at, or null to keep the list's first language.</returns>
        public static string? Choose(IReadOnlyList<VoiceLanguage> languages, string? rememberedLocaleName, string userCulture)
        {
            //  Keep the language the user had before, if the list still has it.
            if (!string.IsNullOrEmpty(rememberedLocaleName) && languages.Any(language => language.LocaleName == rememberedLocaleName))
            {
                return rememberedLocaleName;
            }

            //  Otherwise use the user's own language and region, for example Greek as spoken in Greece.
            VoiceLanguage? exact = languages.FirstOrDefault(language => string.Equals(language.Locale, userCulture, StringComparison.OrdinalIgnoreCase));
            if (exact != null) { return exact.LocaleName; }

            //  Otherwise use the user's language as spoken anywhere, for example English when only British English is listed.
            string userLanguage = userCulture.Split('-')[0];
            if (userLanguage.Length == 0) { return null; }
            VoiceLanguage? sameLanguage = languages.FirstOrDefault(language => language.Locale.Split('-')[0].Equals(userLanguage, StringComparison.OrdinalIgnoreCase));
            return sameLanguage?.LocaleName;
        }
    }
}
