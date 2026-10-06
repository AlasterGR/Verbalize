using Microsoft.CognitiveServices.Speech;
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
        public static Task<SpeechSynthesisResult>? spokenTextSoundResult;
        public static CancellationTokenSource synthesisCancellationToken;
        public static SpeechSynthesisResult speechSynthesisResult = null;
        public static SpeechSynthesizer speechSynthesizer;
        /// <summary> A stand-in key used when none is set, because the speech service refuses an empty one. Requests made with it fail with a clear error instead of the app failing to open. </summary>
        private const string MissingKeyPlaceholder = "not-configured";

        /// <summary> Prepares the connection to Azure's speech service with the user's key and region. </summary>
        public static void Initialize()
        {
            //  Use the user's key and region, or a stand-in key if none is set, so the app can still open.
            string subscriptionKey = Handler_Data.GetTheSubscriptionKey();
            string serverLocation = Handler_Data.GetTheServerLocation();
            config = SpeechConfig.FromSubscription(string.IsNullOrEmpty(subscriptionKey) ? MissingKeyPlaceholder : subscriptionKey, serverLocation);

            //  Prepare the speech maker and the means to cancel it.
            speechSynthesizer = new SpeechSynthesizer(config);
            synthesisCancellationToken = new CancellationTokenSource();
        }
        public static SpeechConfig GetConfig()
        {
            return config;
        }
        public static void SetSpeechSynthesisLanguage(string _speechSynthesisLanguage)
        {
            config.SpeechSynthesisLanguage = _speechSynthesisLanguage;
        }
        public static string GetSpeechSynthesisLanguage()
        {
            return config.SpeechSynthesisLanguage;
        }
        public static void SetSpeechSynthesisVoiceName(string _SpeechSynthesisVoiceName)
        {
            config.SpeechSynthesisVoiceName = _SpeechSynthesisVoiceName;
        }
        public static string GetSpeechSynthesisVoiceName()
        {
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

            //  Stop any speech that is playing, and prepare to make the speech in memory rather than play it.
            SoundPause();
            if (!_audioOn) { speechSynthesizer = new SpeechSynthesizer(config, null); }
            else { speechSynthesizer = new SpeechSynthesizer(config, null); }

            //  Ask Azure to make the speech, and stop with its explanation if it could not.
            speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(ssmlText);
            EnsureSynthesisCompleted(speechSynthesisResult);

            //  Save the speech next to the original file, with the sound format's extension.
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            SaveAudio(speechSynthesisResult.AudioData, outputFile, formatOutputSound);
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
            //  Stop any speech that is playing, and prepare to make the speech in memory rather than play it.
            string ssmlText = _xmlDoc.OuterXml;
            SoundPause();
            if (!_audioOn) { speechSynthesizer = new SpeechSynthesizer(config, null); }
            else { speechSynthesizer = new SpeechSynthesizer(config, null); }

            //  Ask Azure to make the speech, and stop with its explanation if it could not.
            SpeechSynthesisResult result = await speechSynthesizer.SpeakSsmlAsync(ssmlText);
            EnsureSynthesisCompleted(result);

            //  Save the speech with the sound format's extension.
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            SaveAudio(result.AudioData, outputFile, formatOutputSound);
        }

        /// <summary> Stops with Azure's explanation when it could not make the speech. </summary>
        /// <param name="result">Azure's answer to a request for speech.</param>
        /// <exception cref="InvalidOperationException">Azure could not create the speech.</exception>
        private static void EnsureSynthesisCompleted(SpeechSynthesisResult result)
        {
            //  Carry on if Azure finished making the speech.
            if (result.Reason == ResultReason.SynthesizingAudioCompleted) { return; }

            //  Stop with a general message if Azure gave no reason.
            if (result.Reason != ResultReason.Canceled)
            {
                throw new InvalidOperationException($"Azure did not finish creating the speech ({result.Reason}).");
            }

            //  Otherwise stop, passing on Azure's reason and details.
            SpeechSynthesisCancellationDetails details = SpeechSynthesisCancellationDetails.FromResult(result);
            throw new InvalidOperationException($"Azure could not create the speech ({details.Reason}, {details.ErrorCode}): {details.ErrorDetails}");
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

        public static void SoundPause()
        {
            if (spokenTextSoundResult != null && !spokenTextSoundResult.IsCompleted) // Check if there's an ongoing synthesis
            {
                try
                {
                    speechSynthesizer?.StopSpeakingAsync();
                    speechSynthesizer?.Dispose();
                    //spokenTextSoundResult = null; // Reset the ongoing task
                    spokenTextSoundResult?.Dispose();
                }
                catch (Exception ex)
                {

                }
                //implement exception
            }
        }

        public static void SpeakFromTextBox(TextBox _mainSingleTextBox)
        {
            SoundPause();
            speechSynthesizer = new SpeechSynthesizer(config);
            spokenTextSoundResult = speechSynthesizer.SpeakSsmlAsync(Handler_Data.CreateSSML(string.IsNullOrEmpty(_mainSingleTextBox.SelectedText) ? _mainSingleTextBox.Text : _mainSingleTextBox.SelectedText).OuterXml); // Create SSML from textbox's either selected text or entire text (whichever is nonempty) and feed it to the syntesizer
        }
    }
}
