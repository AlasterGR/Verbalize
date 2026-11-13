using Verbalize.Core;

namespace Verbalize.Core.Tests
{
    /// <summary> Checks the hints shown when the pointer rests on a control. </summary>
    public class ControlHintsTests
    {
        /// <summary> The language list's hint names the chosen language's code, which no longer shows on screen. </summary>
        [Fact]
        public void LanguageCode_NamesTheCode()
        {
            //  Build the hint for American English.
            string hint = ControlHints.LanguageCode("en-US");

            //  Check it names the code plainly.
            Assert.Equal("Language code: en-US", hint);
        }
    }
}
