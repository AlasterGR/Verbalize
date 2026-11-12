using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
// for the audio conversion - add an ogg vorbis encoder
using NAudio.MediaFoundation;
using NAudio.Wave;
using System.Xml;
using Verbalize.Core;

namespace _Verbalize
{
    internal class Handler_AudioSynthesis
    {
        /// <summary>  This is the single most valuable object of the app, as it holds all the important properties for the speech synthesis </summary>
        private static SpeechConfig config;
        /// <summary> The speech currently playing aloud, or null when nothing is being spoken. </summary>
        private static SpokenSpeech? currentSpeech;

        /// <summary> Speech playing aloud: the speech maker playing it and, once asked, the request to stop it. </summary>
        /// <param name="synthesizer">The speech maker playing the speech.</param>
        private sealed class SpokenSpeech(SpeechSynthesizer synthesizer)
        {
            /// <summary> The speech maker playing the speech. </summary>
            public SpeechSynthesizer Synthesizer { get; } = synthesizer;
            /// <summary> The request to stop the speech, or null if it has not been asked to stop. </summary>
            public Task? StopRequest { get; set; }
        }
        /// <summary> A stand-in key used when none is set, because the speech service refuses an empty one. Requests made with it fail with a clear error instead of the app failing to open. </summary>
        private const string MissingKeyPlaceholder = "not-configured";

        /// <summary> Prepares the connection to Azure's speech service with the user's key and region. </summary>
        public static void Initialize()
        {
            //  Use the user's key and region, or a stand-in key if none is set, so the app can still open.
            string subscriptionKey = Handler_Data.GetTheSubscriptionKey();
            string serverLocation = Handler_Data.GetTheServerLocation();
            config = SpeechConfig.FromSubscription(string.IsNullOrEmpty(subscriptionKey) ? MissingKeyPlaceholder : subscriptionKey, serverLocation);
        }

        /// <summary> Returns the speech settings: key, region, language and voice. </summary>
        /// <returns>The speech settings.</returns>
        public static SpeechConfig GetConfig()
        {
            //  Give back the shared settings.
            return config;
        }

        /// <summary> Sets the language the speech is made in. </summary>
        /// <param name="_speechSynthesisLanguage">The language code, for example "en-US".</param>
        public static void SetSpeechSynthesisLanguage(string _speechSynthesisLanguage)
        {
            //  Store the language in the speech settings.
            config.SpeechSynthesisLanguage = _speechSynthesisLanguage;
        }

        /// <summary> Returns the language the speech is made in. </summary>
        /// <returns>The language code, for example "en-US".</returns>
        public static string GetSpeechSynthesisLanguage()
        {
            //  Read the language from the speech settings.
            return config.SpeechSynthesisLanguage;
        }

        /// <summary> Sets the voice that speaks. </summary>
        /// <param name="_SpeechSynthesisVoiceName">The voice's short name, for example "en-US-JennyNeural".</param>
        public static void SetSpeechSynthesisVoiceName(string _SpeechSynthesisVoiceName)
        {
            //  Store the voice in the speech settings.
            config.SpeechSynthesisVoiceName = _SpeechSynthesisVoiceName;
        }

        /// <summary> Returns the voice that speaks. </summary>
        /// <returns>The voice's short name, for example "en-US-JennyNeural".</returns>
        public static string GetSpeechSynthesisVoiceName()
        {
            //  Read the voice from the speech settings.
            return config.SpeechSynthesisVoiceName;
        }
        /// <summary> Turns a file into speech and saves it as a sound file next to it, with the same name. </summary>
        /// <param name="soundfile">The file to speak: SSML, or plain text, which is spoken with the app's current voice settings.</param>
        /// <param name="formatOutputSound">The sound format to save as: "mp3", "wav", or anything else to save nothing.</param>
        /// <param name="_audioOn">Not used yet; the speech is never played aloud.</param>
        /// <returns>A task that finishes once the sound file is saved.</returns>
        /// <exception cref="InvalidOperationException">Azure could not create the speech.</exception>
        public static async Task SynthesizeAudioAsync(string soundfile, string formatOutputSound, bool _audioOn)
        {
            //  Read the chosen file as SSML, wrapping it with the current voice settings if it is plain text.
            XmlDocument xmlDoc = SsmlFileLoader.Load(soundfile, Handler_Data.CreateSSML);
            string ssmlText = xmlDoc.OuterXml;

            //  Make the speech and save it next to the original file, with the sound format's extension.
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            await SynthesizeToFileAsync(ssmlText, outputFile, formatOutputSound);
        }

        /// <summary> Turns an SSML document into speech and saves it as a sound file. </summary>
        /// <param name="_xmlDoc">The SSML document to speak.</param>
        /// <param name="soundfile">Where to save the sound; its extension is replaced by the sound format's.</param>
        /// <param name="formatOutputSound">The sound format to save as: "mp3", "wav", or anything else to save nothing.</param>
        /// <param name="_audioOn">Not used yet; the speech is never played aloud.</param>
        /// <returns>A task that finishes once the sound file is saved.</returns>
        /// <exception cref="InvalidOperationException">Azure could not create the speech.</exception>
        public static async Task SynthesizeAudioAsyncFromText(XmlDocument _xmlDoc, string soundfile, string formatOutputSound, bool _audioOn)
        {
            //  Make the speech and save it with the sound format's extension.
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            await SynthesizeToFileAsync(_xmlDoc.OuterXml, outputFile, formatOutputSound);
        }

        /// <summary> Stops any speech playing aloud, then makes the speech in memory and saves it as a sound file. </summary>
        /// <param name="ssmlText">The SSML to speak.</param>
        /// <param name="outputFile">Where to save the sound file.</param>
        /// <param name="formatOutputSound">The sound format: "mp3", "wav", or anything else to save nothing.</param>
        /// <returns>A task that finishes once the sound file is saved.</returns>
        /// <exception cref="InvalidOperationException">Azure could not create the speech.</exception>
        private static async Task SynthesizeToFileAsync(string ssmlText, string outputFile, string formatOutputSound)
        {
            //  Stop any speech that is playing aloud.
            await StopSpeakingAsync();

            //  Ask Azure to make the speech in memory rather than play it, and stop with its explanation if it could not.
            using SpeechSynthesizer synthesizer = new(config, null as AudioConfig);
            using SpeechSynthesisResult result = await synthesizer.SpeakSsmlAsync(ssmlText);
            string? failure = DescribeFailure(result);
            if (failure != null) { throw new InvalidOperationException(failure); }

            //  Save the speech as a sound file.
            SaveAudio(result.AudioData, outputFile, formatOutputSound);
        }

        /// <summary> Explains in plain words why Azure could not make the speech. </summary>
        /// <param name="result">Azure's answer to a request for speech.</param>
        /// <returns>The explanation, or null if the speech was made.</returns>
        private static string? DescribeFailure(SpeechSynthesisResult result)
        {
            //  Nothing to explain if Azure finished making the speech.
            if (result.Reason == ResultReason.SynthesizingAudioCompleted) { return null; }

            //  Give a general explanation if Azure did not say it was cancelled.
            bool hasKey = Handler_Data.GetSpeechCredentials().HasKey;
            if (result.Reason != ResultReason.Canceled)
            {
                return SpeechFailureMessage.Build(hasKey, null, $"Azure stopped with the result {result.Reason}.");
            }

            //  Otherwise explain using Azure's own error code and details.
            SpeechSynthesisCancellationDetails details = SpeechSynthesisCancellationDetails.FromResult(result);
            return SpeechFailureMessage.Build(hasKey, details.ErrorCode.ToString(), details.ErrorDetails);
        }

        /// <summary> Saves speech made by Azure as a sound file. </summary>
        /// <param name="audioData">The speech, as Azure returned it (a WAV recording).</param>
        /// <param name="outputFile">Where to save the sound file.</param>
        /// <param name="formatOutputSound">The sound format: "mp3", "wav", or anything else to save nothing.</param>
        private static void SaveAudio(byte[] audioData, string outputFile, string formatOutputSound)
        {
            //  Read the speech as a WAV recording.
            using Stream stream = new MemoryStream(audioData);
            switch (formatOutputSound)
            {
                //  Convert it to MP3 using Windows' built-in encoder.
                case "mp3":
                    MediaFoundationApi.Startup();
                    var reader = new WaveFileReader(stream);
                    MediaFoundationEncoder.EncodeToMp3(reader, outputFile);
                    break;

                //  Save it as a WAV file as it is.
                case "wav":
                    MediaFoundationApi.Startup();
                    var reader1 = new WaveFileReader(stream);
                    WaveFileWriter.CreateWaveFile(outputFile, reader1);
                    break;

                //  Save nothing for OGG (not supported yet) or any other choice.
                case "ogg":
                    break;
                default:
                    break;
            }
        }

        /// <summary> Stops the speech that is playing aloud, if any, and waits until it has stopped. </summary>
        /// <returns>A task that finishes once the speech has stopped.</returns>
        public static async Task StopSpeakingAsync()
        {
            //  Do nothing if nothing is being spoken.
            SpokenSpeech? speech = currentSpeech;
            if (speech == null) { return; }

            //  Mark the speech as stopped, so its end is not reported as a failure, then stop it and wait.
            currentSpeech = null;
            speech.StopRequest = speech.Synthesizer.StopSpeakingAsync();
            try { await speech.StopRequest; }
            //  Ignore a failure to stop, since the speech maker is closed straight after anyway.
            catch (Exception) { }
        }

        /// <summary> Speaks the main text box's selected text aloud, or all of its text if none is selected, and tells the user if it fails. </summary>
        /// <param name="_mainSingleTextBox">The main text box.</param>
        /// <returns>A task that finishes once the speech has ended, been stopped, or the user has been told it failed.</returns>
        public static async Task SpeakFromTextBoxAsync(TextBox _mainSingleTextBox)
        {
            //  Stop any speech that is already playing.
            await StopSpeakingAsync();

            //  Wrap the selected text, or all the text if none is selected, with the current voice settings.
            string text = string.IsNullOrEmpty(_mainSingleTextBox.SelectedText) ? _mainSingleTextBox.Text : _mainSingleTextBox.SelectedText;
            string ssmlText = Handler_Data.CreateSSML(text).OuterXml;

            //  Speak it aloud through the speakers, remembering the speech so it can be stopped.
            using SpeechSynthesizer synthesizer = new(config);
            SpokenSpeech speech = new(synthesizer);
            currentSpeech = speech;
            string? failure;
            try
            {
                using SpeechSynthesisResult result = await synthesizer.SpeakSsmlAsync(ssmlText);
                failure = DescribeFailure(result);
            }
            //  Treat an unexpected error from the speech service like any other failure.
            catch (Exception exception)
            {
                failure = exception.Message;
            }

            //  Forget the speech if it ended by itself.
            if (currentSpeech == speech) { currentSpeech = null; }

            //  If it was stopped, wait for the stop to finish before closing the speech maker, and say nothing.
            if (speech.StopRequest != null)
            {
                try { await speech.StopRequest; } catch (Exception) { }
                return;
            }

            //  Say nothing more if the speech finished normally.
            if (failure == null) { return; }

            //  Otherwise tell the user why the text could not be spoken.
            MessageBox.Show("The text could not be spoken." + Environment.NewLine + Environment.NewLine + failure,
                Form1.applicationBrandName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
