using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks how the app's own title bar behaves, and how its buttons read. </summary>
    public class TitleBarTests
    {
        /// <summary> Empty parts of the title bar move the window, and clickable parts keep their clicks. </summary>
        [Theory]
        [InlineData(TitleBarTarget.Empty, TitleBarArea.Caption)]
        [InlineData(TitleBarTarget.Control, TitleBarArea.Client)]
        [InlineData(TitleBarTarget.MaximizeButton, TitleBarArea.MaximizeButton)]
        [InlineData(TitleBarTarget.OutsideTitleBar, TitleBarArea.Client)]
        public void Decide_BelowTheTopEdge_FollowsWhatIsUnderThePointer(TitleBarTarget target, TitleBarArea expected)
        {
            //  Decide for a point well below the top edge.
            TitleBarArea area = TitleBarHitTest.Decide(500, 20, 1000, 8, false, target);

            //  Check it follows what is under the pointer.
            Assert.Equal(expected, area);
        }

        /// <summary> The top edge resizes the window, with corners twice as wide as the edge is thick. </summary>
        [Theory]
        [InlineData(0, TitleBarArea.TopLeft)]
        [InlineData(15, TitleBarArea.TopLeft)]
        [InlineData(16, TitleBarArea.Top)]
        [InlineData(983, TitleBarArea.Top)]
        [InlineData(984, TitleBarArea.TopRight)]
        public void Decide_OnTheTopEdge_Resizes(int x, TitleBarArea expected)
        {
            //  Decide for a point on the top edge, even over a button.
            TitleBarArea area = TitleBarHitTest.Decide(x, 3, 1000, 8, false, TitleBarTarget.Control);

            //  Check it resizes from the right side or corner.
            Assert.Equal(expected, area);
        }

        /// <summary> A maximised window has no edges to resize, so the very top stays part of the title bar. </summary>
        [Fact]
        public void Decide_WhenMaximized_DoesNotResize()
        {
            //  Decide for the very top of a maximised window, over an empty part and over a button.
            TitleBarArea empty = TitleBarHitTest.Decide(500, 0, 1000, 8, true, TitleBarTarget.Empty);
            TitleBarArea button = TitleBarHitTest.Decide(990, 0, 1000, 8, true, TitleBarTarget.Control);

            //  Check the empty part moves the window and the button keeps its click.
            Assert.Equal(TitleBarArea.Caption, empty);
            Assert.Equal(TitleBarArea.Client, button);
        }

        /// <summary> The Maximise button offers to maximise a normal window and to restore a maximised one, and its menu item matches. </summary>
        [Fact]
        public void DescribeMaximize_OffersTheOppositeSize()
        {
            //  Describe the button for both window sizes.
            MaximizeButtonState normal = WindowButtons.DescribeMaximize(false);
            MaximizeButtonState maximized = WindowButtons.DescribeMaximize(true);

            //  Check each offers the other size, with matching names.
            Assert.Equal(("", "Maximize", ControlHints.Maximize, "Maximize"), (normal.Glyph, normal.Name, normal.Hint, normal.MenuText));
            Assert.Equal(("", "Restore", ControlHints.Restore, "Restore"), (maximized.Glyph, maximized.Name, maximized.Hint, maximized.MenuText));
        }

        /// <summary> Help names the shortcuts and the key setup, and stays until closed. </summary>
        [Fact]
        public void Help_NamesShortcutsAndKeySetup()
        {
            //  Build the help message.
            UserMessage help = UserMessages.Help();

            //  Check it mentions the shortcuts and the key, and does not close by itself.
            Assert.Contains("Ctrl+Enter", help.Text);
            Assert.Contains("Esc", help.Text);
            Assert.Contains("VERBALIZE_SPEECH_KEY", help.Text);
            Assert.Equal(UserMessages.HelpTopic, help.Topic);
            Assert.False(help.ClosesByItself);
        }
    }
}
