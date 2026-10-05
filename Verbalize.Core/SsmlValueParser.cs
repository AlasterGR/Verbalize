using System.Globalization;
using System.Text.RegularExpressions;

namespace Verbalize.Core
{
    /// <summary> Reads numbers out of SSML attribute values such as "+10Hz", "-20%" or "80". </summary>
    public static partial class SsmlValueParser
    {
        /// <summary> Tries to read a whole number from a value, ignoring any unit around it, so "+10Hz" gives 10 and "-20%" gives -20. </summary>
        /// <param name="value">The attribute value to read.</param>
        /// <param name="number">The number that was read, or 0 if none could be read.</param>
        /// <returns>True if a number could be read.</returns>
        public static bool TryParseSignedInteger(string value, out int number)
        {
            //  Remove everything except digits and plus or minus signs, then read what is left as a number.
            string digitsAndSigns = NonNumericCharacters().Replace(value, string.Empty);
            return int.TryParse(digitsAndSigns, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out number);
        }

        /// <summary> Tries to read a volume from a value, keeping it within the allowed range of 0 to 100. </summary>
        /// <param name="value">The volume attribute value to read, for example "80".</param>
        /// <param name="volume">The volume that was read, limited to 0 to 100, or 0 if none could be read.</param>
        /// <returns>True if a volume could be read.</returns>
        public static bool TryParseVolume(string value, out int volume)
        {
            //  Give up if the value holds no readable number.
            if (!TryParseSignedInteger(value, out int parsedVolume))
            {
                volume = 0;
                return false;
            }

            //  Keep the volume between silent (0) and loudest (100).
            volume = Math.Clamp(parsedVolume, 0, 100);
            return true;
        }

        /// <summary> Matches every character that is not a digit or a plus or minus sign. </summary>
        [GeneratedRegex("[^0-9-+]")]
        private static partial Regex NonNumericCharacters();
    }
}
