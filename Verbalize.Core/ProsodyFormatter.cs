using System.Globalization;

namespace Verbalize.Core
{
    /// <summary> Turns the app's slider positions into prosody values Azure understands. </summary>
    public static class ProsodyFormatter
    {
        /// <summary> Turns a pitch change in hertz into an SSML value such as "+10Hz" or "-10Hz". </summary>
        /// <param name="hertz">How far to raise (positive) or lower (negative) the voice, in hertz.</param>
        /// <returns>The pitch value, always with its sign.</returns>
        /// <remarks> Azure reads a value without a sign, such as "10Hz", as an absolute pitch of 10 Hz rather than a change, so the sign is always included, even for zero. </remarks>
        public static string FormatRelativePitch(int hertz)
        {
            //  Write the number with a plus or minus sign in front, followed by "Hz".
            return hertz.ToString("+0;-0;+0", CultureInfo.InvariantCulture) + "Hz";
        }
    }
}
