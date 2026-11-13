using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the single Speak/Stop button. </summary>
    public class SpeakButtonTests
    {
        /// <summary> While speaking, the button reads Stop and always works, even with no text, no connection and no key. </summary>
        [Fact]
        public void Describe_WhileSpeaking_OffersStopAlways()
        {
            //  Describe the button while speaking in the worst conditions.
            SpeakButtonState state = SpeakButton.Describe(true, new AppConditions(false, false, false));

            //  Check it offers a working Stop with the Stop hint.
            Assert.Equal("St&op", state.Text);
            Assert.True(state.Enabled);
            Assert.True(state.Stops);
            Assert.Equal(ControlHints.Stop, state.Hint);
        }

        /// <summary> When idle, the button reads Speak and works only when the text can be spoken. </summary>
        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, true, false, false)]
        public void Describe_WhenIdle_OffersSpeakWhenPossible(bool hasText, bool isOnline, bool hasKey, bool expectedEnabled)
        {
            //  Describe the button when nothing is being spoken.
            SpeakButtonState state = SpeakButton.Describe(false, new AppConditions(hasText, isOnline, hasKey));

            //  Check it offers Speak, enabled only when everything needed is there.
            Assert.Equal("Sp&eak", state.Text);
            Assert.False(state.Stops);
            Assert.Equal(expectedEnabled, state.Enabled);
            Assert.Equal(ControlHints.Speak, state.Hint);
        }
    }
}
