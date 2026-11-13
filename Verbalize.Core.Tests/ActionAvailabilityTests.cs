using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks which actions the app offers in each situation. </summary>
    public class ActionAvailabilityTests
    {
        /// <summary> Only text with something other than spaces and line breaks counts as speakable. </summary>
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("\r\n\t", false)]
        [InlineData("Hello", true)]
        [InlineData("  a  ", true)]
        public void HasSpeakableText_IgnoresBlankText(string? text, bool expected)
        {
            //  Check the text.
            bool speakable = ActionAvailability.HasSpeakableText(text);

            //  Check the answer.
            Assert.Equal(expected, speakable);
        }

        /// <summary> Azure can be reached only when online with a key, whatever the text. </summary>
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        public void CanReachAzure_NeedsConnectionAndKey(bool isOnline, bool hasKey, bool expected)
        {
            //  Check with no text, which should not matter.
            bool canReach = ActionAvailability.CanReachAzure(new AppConditions(false, isOnline, hasKey));

            //  Check the answer.
            Assert.Equal(expected, canReach);
        }

        /// <summary> Speaking needs text, a connection and a key, all three. </summary>
        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, false, false)]
        public void CanSpeakText_NeedsTextConnectionAndKey(bool hasText, bool isOnline, bool hasKey, bool expected)
        {
            //  Check the situation.
            bool canSpeak = ActionAvailability.CanSpeakText(new AppConditions(hasText, isOnline, hasKey));

            //  Check the answer.
            Assert.Equal(expected, canSpeak);
        }
    }
}
