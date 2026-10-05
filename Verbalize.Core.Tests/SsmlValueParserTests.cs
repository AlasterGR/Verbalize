using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks that numbers are read correctly from values such as "+10Hz" and "-20%". </summary>
    public class SsmlValueParserTests
    {
        /// <summary> Units are ignored and signs are kept. </summary>
        [Theory]
        [InlineData("+10Hz", 10)]
        [InlineData("-10Hz", -10)]
        [InlineData("20%", 20)]
        [InlineData("-50%", -50)]
        [InlineData("0", 0)]
        [InlineData(" 7 st ", 7)]
        public void TryParseSignedInteger_ReadsTheNumber(string value, int expected)
        {
            //  Read the value.
            bool parsed = SsmlValueParser.TryParseSignedInteger(value, out int number);

            //  Check a number was read and is correct.
            Assert.True(parsed);
            Assert.Equal(expected, number);
        }

        /// <summary> Values with no usable number are rejected. </summary>
        [Theory]
        [InlineData("")]
        [InlineData("default")]
        [InlineData("x-high")]
        [InlineData("+-5Hz")]
        public void TryParseSignedInteger_RejectsValuesWithoutANumber(string value)
        {
            //  Try to read the value.
            bool parsed = SsmlValueParser.TryParseSignedInteger(value, out int number);

            //  Check it was rejected.
            Assert.False(parsed);
            Assert.Equal(0, number);
        }

        /// <summary> Volumes are kept within 0 to 100. </summary>
        [Theory]
        [InlineData("80", 80)]
        [InlineData("100", 100)]
        [InlineData("101", 100)]
        [InlineData("-1", 0)]
        [InlineData("+25", 25)]
        public void TryParseVolume_KeepsTheVolumeInRange(string value, int expected)
        {
            //  Read the volume.
            bool parsed = SsmlValueParser.TryParseVolume(value, out int volume);

            //  Check it was read and limited to the allowed range.
            Assert.True(parsed);
            Assert.Equal(expected, volume);
        }

        /// <summary> A volume with no number is rejected. </summary>
        [Fact]
        public void TryParseVolume_RejectsAVolumeWithoutANumber()
        {
            //  Try to read a volume given as a word.
            bool parsed = SsmlValueParser.TryParseVolume("loud", out int volume);

            //  Check it was rejected.
            Assert.False(parsed);
            Assert.Equal(0, volume);
        }
    }
}
