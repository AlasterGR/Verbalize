namespace Verbalize.Core
{
    /// <summary> Turns the reason Azure gives for failing to make speech into a message the user can act on. </summary>
    public static class SpeechFailureMessage
    {
        /// <summary> Builds a plain-language message saying why the speech could not be made and what to do about it. </summary>
        /// <param name="hasKey">True if an Azure Speech key is set.</param>
        /// <param name="errorCode">Azure's error code, such as "AuthenticationFailure", or null if it gave none.</param>
        /// <param name="errorDetails">Azure's own description of the error, or null if it gave none.</param>
        /// <returns>The message to show the user.</returns>
        public static string Build(bool hasKey, string? errorCode, string? errorDetails)
        {
            //  Explain how to set the key when none is set, since nothing else can work without it.
            if (!hasKey)
            {
                return "No Azure Speech key is set, so the app cannot speak yet." + Environment.NewLine + Environment.NewLine
                    + "Set the " + SpeechCredentialsResolver.KeyVariable + " environment variable, or put the key in the settings file, then restart the app.";
            }

            //  Pick an explanation that matches Azure's error code.
            string explanation = errorCode switch
            {
                "AuthenticationFailure" => "Azure did not accept the Speech key. Check that the key is correct and that the region matches the one shown for it in the Azure portal.",
                "Forbidden" => "Azure refused the request. The Speech resource may be disabled, or the key may belong to a different region.",
                "TooManyRequests" => "Azure is receiving too many requests, or the free monthly allowance has been used up. Try again later.",
                "BadRequest" => "Azure could not read the request. The chosen voice, style or speech settings may not be valid together.",
                "ConnectionFailure" => "The app could not reach Azure. Check the internet connection.",
                "ServiceTimeout" => "Azure took too long to answer. Try again.",
                "ServiceError" or "ServiceUnavailable" => "Azure's speech service had a problem. Try again later.",
                _ => "Azure could not create the speech.",
            };

            //  Add Azure's own description underneath, when it gave one.
            return string.IsNullOrWhiteSpace(errorDetails)
                ? explanation
                : explanation + Environment.NewLine + Environment.NewLine + errorDetails.Trim();
        }
    }
}
