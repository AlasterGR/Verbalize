using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the messages shown in the app's message bar. </summary>
    public class UserMessagesTests
    {
        /// <summary> Where the settings file would be, for the tests. </summary>
        private const string SettingsPath = @"C:\Users\me\AppData\Roaming\Verbalize\settings.json";

        /// <summary> The offline message is a lasting warning that names what is paused. </summary>
        [Fact]
        public void Offline_IsALastingWarning()
        {
            //  Build the offline message.
            UserMessage message = UserMessages.Offline();

            //  Check it is a warning tied to the offline topic, so it can be removed when back online.
            Assert.Equal(MessageSeverity.Warning, message.Severity);
            Assert.Equal(UserMessages.OfflineTopic, message.Topic);
            Assert.False(message.ClosesByItself);
            Assert.Contains("Speaking", message.Text);
        }

        /// <summary> No setup message is needed when a key is set and the settings file is fine. </summary>
        [Fact]
        public void SpeechSetup_WithKeyAndNoProblem_GivesNothing()
        {
            //  Build the message for a working setup.
            UserMessage? message = UserMessages.SpeechSetup(new SpeechCredentials("key", "westeurope", null), SettingsPath);

            //  Check there is nothing to say.
            Assert.Null(message);
        }

        /// <summary> Without a key, the message explains both ways to set one, with the region filled in. </summary>
        [Fact]
        public void SpeechSetup_WithoutKey_ExplainsHowToSetIt()
        {
            //  Build the message for a missing key.
            UserMessage? message = UserMessages.SpeechSetup(new SpeechCredentials(null, "northeurope", null), SettingsPath);

            //  Check it names the variable, the settings file and the region, as a lasting warning.
            Assert.NotNull(message);
            Assert.Equal(MessageSeverity.Warning, message.Severity);
            Assert.Equal(UserMessages.SpeechSetupTopic, message.Topic);
            Assert.Contains(SpeechCredentialsResolver.KeyVariable, message.Text);
            Assert.Contains(SettingsPath, message.Text);
            Assert.Contains("\"northeurope\"", message.Text);
        }

        /// <summary> With a key but a damaged settings file, only the problem is reported. </summary>
        [Fact]
        public void SpeechSetup_WithKeyAndProblem_ReportsOnlyTheProblem()
        {
            //  Build the message for a working key but a damaged settings file.
            UserMessage? message = UserMessages.SpeechSetup(new SpeechCredentials("key", "westeurope", "The settings file could not be read."), SettingsPath);

            //  Check only the problem is shown.
            Assert.NotNull(message);
            Assert.Equal("The settings file could not be read.", message.Text);
        }

        /// <summary> Without a key and with a damaged settings file, both are reported, on separate lines. </summary>
        [Fact]
        public void SpeechSetup_WithoutKeyAndWithProblem_ReportsBoth()
        {
            //  Build the message for a missing key and a damaged settings file.
            UserMessage? message = UserMessages.SpeechSetup(new SpeechCredentials(null, "westeurope", "The settings file could not be read."), SettingsPath);

            //  Check the problem follows the setup explanation on a new line.
            Assert.NotNull(message);
            Assert.StartsWith("No Azure Speech key is set", message.Text);
            Assert.EndsWith(Environment.NewLine + "The settings file could not be read.", message.Text);
        }

        /// <summary> Failures are errors that stay until dismissed, leading with what failed. </summary>
        [Fact]
        public void Failures_AreErrorsThatStay()
        {
            //  Build each failure message.
            UserMessage[] messages =
            [
                UserMessages.SpeakFailed("No connection."),
                UserMessages.SoundFileFailed("No connection."),
                UserMessages.VoiceListDownloadFailed("No connection."),
            ];

            //  Check each is an error that does not close by itself and ends with the reason.
            Assert.All(messages, message =>
            {
                Assert.Equal(MessageSeverity.Error, message.Severity);
                Assert.False(message.ClosesByItself);
                Assert.EndsWith(" No connection.", message.Text);
            });
        }

        /// <summary> Success messages close by themselves, and the saved file is named. </summary>
        [Fact]
        public void Successes_CloseByThemselves()
        {
            //  Build the success messages.
            UserMessage saved = UserMessages.SoundFileSaved(@"C:\out\speech.mp3");
            UserMessage downloaded = UserMessages.VoiceListDownloaded();

            //  Check both close by themselves and the file is named.
            Assert.True(saved.ClosesByItself);
            Assert.True(downloaded.ClosesByItself);
            Assert.Contains(@"C:\out\speech.mp3", saved.Text);
        }

        /// <summary> The hint to choose a sound format is information that closes by itself. </summary>
        [Fact]
        public void ChooseSoundFormat_IsInformation()
        {
            //  Build the hint.
            UserMessage message = UserMessages.ChooseSoundFormat();

            //  Check it is information that closes by itself.
            Assert.Equal(MessageSeverity.Information, message.Severity);
            Assert.True(message.ClosesByItself);
        }

        /// <summary> Information tied to a topic stays, because its situation is still going on. </summary>
        [Fact]
        public void ClosesByItself_IsFalseForTopicMessages()
        {
            //  Build an information message tied to a topic.
            UserMessage message = new(MessageSeverity.Information, "Still going on.", "topic");

            //  Check it does not close by itself.
            Assert.False(message.ClosesByItself);
        }
    }
}
