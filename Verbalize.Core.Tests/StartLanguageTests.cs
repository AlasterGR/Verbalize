using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks which language the language list starts at. </summary>
    public class StartLanguageTests
    {
        /// <summary> A short list in the order Azure gives it, starting with Afrikaans. </summary>
        private static readonly VoiceLanguage[] Languages =
        {
            new("af-ZA", "Afrikaans (South Africa)"),
            new("el-GR", "Greek (Greece)"),
            new("en-GB", "English (United Kingdom)"),
            new("en-US", "English (United States)"),
        };

        /// <summary> The language the user had before wins over their Windows language. </summary>
        [Fact]
        public void Choose_PrefersTheRememberedLanguage()
        {
            //  Choose for a Greek user who last used British English.
            string? chosen = StartLanguage.Choose(Languages, "English (United Kingdom)", "el-GR");

            //  Check British English is kept.
            Assert.Equal("English (United Kingdom)", chosen);
        }

        /// <summary> With nothing remembered, the user's own language and region is chosen. </summary>
        [Fact]
        public void Choose_UsesTheUsersLanguageAndRegion()
        {
            //  Choose for a Greek user on the first run.
            string? chosen = StartLanguage.Choose(Languages, null, "el-GR");

            //  Check Greek is chosen.
            Assert.Equal("Greek (Greece)", chosen);
        }

        /// <summary> A remembered language that is no longer listed is ignored. </summary>
        [Fact]
        public void Choose_IgnoresARememberedLanguageThatIsGone()
        {
            //  Choose for a US user who last used a language the list no longer has.
            string? chosen = StartLanguage.Choose(Languages, "Klingon", "en-US");

            //  Check US English is chosen instead.
            Assert.Equal("English (United States)", chosen);
        }

        /// <summary> When the user's region is not listed, another region of the same language is chosen. </summary>
        [Fact]
        public void Choose_FallsBackToTheSameLanguageInAnotherRegion()
        {
            //  Choose for an Australian English user.
            string? chosen = StartLanguage.Choose(Languages, null, "en-AU");

            //  Check the first listed English is chosen.
            Assert.Equal("English (United Kingdom)", chosen);
        }

        /// <summary> When the user's language is not listed at all, the list keeps its first language. </summary>
        [Fact]
        public void Choose_ReturnsNullWhenNothingMatches()
        {
            //  Choose for a Japanese user, and for a user with no language set.
            string? forJapanese = StartLanguage.Choose(Languages, null, "ja-JP");
            string? forNoLanguage = StartLanguage.Choose(Languages, null, "");

            //  Check neither picks a language.
            Assert.Null(forJapanese);
            Assert.Null(forNoLanguage);
        }
    }
}
