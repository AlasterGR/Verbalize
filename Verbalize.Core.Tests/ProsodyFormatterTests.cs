using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that slider positions are turned into values Azure reads as a change in pitch. </summary>
    public class ProsodyFormatterTests
    {
        /// <summary> Every pitch change carries a sign, because Azure reads an unsigned "10Hz" as an absolute pitch of 10 Hz. </summary>
        [Theory]
        [InlineData(10, "+10Hz")]
        [InlineData(30, "+30Hz")]
        [InlineData(0, "+0Hz")]
        [InlineData(-10, "-10Hz")]
        [InlineData(-30, "-30Hz")]
        public void FormatRelativePitch_AlwaysIncludesTheSign(int hertz, string expected)
        {
            //  Turn the slider position into a pitch value and check it.
            Assert.Equal(expected, ProsodyFormatter.FormatRelativePitch(hertz));
        }

        /// <summary> A formatted pitch reads back as the same slider position when a saved file is loaded. </summary>
        [Theory]
        [InlineData(-30)]
        [InlineData(0)]
        [InlineData(17)]
        public void FormatRelativePitch_ReadsBackAsTheSameNumber(int hertz)
        {
            //  Format the pitch, then read the number back out of it.
            bool parsed = SsmlValueParser.TryParseSignedInteger(ProsodyFormatter.FormatRelativePitch(hertz), out int readBack);

            //  Check the number is unchanged.
            Assert.True(parsed);
            Assert.Equal(hertz, readBack);
        }
    }
}
