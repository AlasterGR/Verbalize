using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the status bar's description of the app's state. </summary>
    public class SystemStatusTests
    {
        /// <summary> When idle, online and with a key, the status reads Ready, Online, Speech key set. </summary>
        [Fact]
        public void Describe_IdleOnlineWithKey()
        {
            //  Describe the usual resting state.
            string status = SystemStatus.Describe(true, true, AppActivity.None);

            //  Check all three parts, in order.
            Assert.Equal("Ready" + SystemStatus.Separator + "Online" + SystemStatus.Separator + "Speech key set", status);
        }

        /// <summary> Being offline and without a key are both stated. </summary>
        [Fact]
        public void Describe_OfflineWithoutKey()
        {
            //  Describe the worst case.
            string status = SystemStatus.Describe(false, false, AppActivity.None);

            //  Check both problems are named.
            Assert.Contains("Offline", status);
            Assert.Contains("No speech key", status);
        }

        /// <summary> Each activity is named first, in place of Ready. </summary>
        [Theory]
        [InlineData(AppActivity.Speaking, "Speaking…")]
        [InlineData(AppActivity.SavingSound, "Saving the sound file…")]
        [InlineData(AppActivity.DownloadingVoices, "Downloading the voice list…")]
        public void Describe_NamesTheActivityFirst(AppActivity activity, string expected)
        {
            //  Describe the app while busy.
            string status = SystemStatus.Describe(true, true, activity);

            //  Check the activity leads and Ready is gone.
            Assert.StartsWith(expected + SystemStatus.Separator, status);
            Assert.DoesNotContain("Ready", status);
        }
    }
}
