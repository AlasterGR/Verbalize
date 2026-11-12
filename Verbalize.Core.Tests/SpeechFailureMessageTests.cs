using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the messages shown when Azure cannot make speech. </summary>
    public class SpeechFailureMessageTests
    {
        /// <summary> Without a key, the message explains how to set one, whatever Azure said. </summary>
        [Fact]
        public void Build_WithoutKey_ExplainsHowToSetIt()
        {
            //  Build the message as if no key were set.
            string message = SpeechFailureMessage.Build(false, "AuthenticationFailure", "401 Unauthorized");

            //  Check it names the environment variable and leaves out Azure's details.
            Assert.Contains("No Azure Speech key is set", message);
            Assert.Contains(SpeechCredentialsResolver.KeyVariable, message);
            Assert.DoesNotContain("401", message);
        }

        /// <summary> Each known error code gets its own explanation. </summary>
        [Theory]
        [InlineData("AuthenticationFailure", "did not accept the Speech key")]
        [InlineData("Forbidden", "refused the request")]
        [InlineData("TooManyRequests", "free monthly allowance")]
        [InlineData("BadRequest", "could not read the request")]
        [InlineData("ConnectionFailure", "internet connection")]
        [InlineData("ServiceTimeout", "too long to answer")]
        [InlineData("ServiceError", "had a problem")]
        [InlineData("ServiceUnavailable", "had a problem")]
        public void Build_KnownErrorCode_GivesMatchingExplanation(string errorCode, string expectedText)
        {
            //  Build the message for the error code.
            string message = SpeechFailureMessage.Build(true, errorCode, null);

            //  Check the matching explanation is used.
            Assert.Contains(expectedText, message);
        }

        /// <summary> An unknown or missing error code gets a general explanation. </summary>
        [Theory]
        [InlineData("RuntimeError")]
        [InlineData(null)]
        public void Build_UnknownErrorCode_GivesGeneralExplanation(string? errorCode)
        {
            //  Build the message for the error code.
            string message = SpeechFailureMessage.Build(true, errorCode, null);

            //  Check the general explanation is used.
            Assert.Equal("Azure could not create the speech.", message);
        }

        /// <summary> Azure's own details are added below the explanation, trimmed. </summary>
        [Fact]
        public void Build_WithDetails_AddsThemBelow()
        {
            //  Build the message with Azure's details.
            string message = SpeechFailureMessage.Build(true, "BadRequest", "  Unsupported voice.  ");

            //  Check the details follow the explanation after an empty line.
            Assert.EndsWith(Environment.NewLine + Environment.NewLine + "Unsupported voice.", message);
        }

        /// <summary> Blank details are left out. </summary>
        [Fact]
        public void Build_WithBlankDetails_LeavesThemOut()
        {
            //  Build the message with blank details.
            string message = SpeechFailureMessage.Build(true, "ServiceTimeout", "   ");

            //  Check only the explanation is shown.
            Assert.Equal("Azure took too long to answer. Try again.", message);
        }
    }
}
