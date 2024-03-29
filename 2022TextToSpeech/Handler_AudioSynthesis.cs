﻿using Microsoft.CognitiveServices.Speech;
// for the audio conversion - add an ogg vorbis encoder
using NAudio.MediaFoundation;
using NAudio.Wave;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace _Verbalize
{
    internal class Handler_AudioSynthesis
    {
        /// <summary>  This is the single most valuable object of the app, as it holds all the important properties for the speech synthesis </summary>
        private static SpeechConfig config ;
        public static Task<SpeechSynthesisResult>? spokenTextSoundResult;
        public static CancellationTokenSource synthesisCancellationToken;
        public static SpeechSynthesisResult speechSynthesisResult = null;
        public static SpeechSynthesizer speechSynthesizer;
        public static void Initialize()
        {
            string subscriptionKey = Handler_Data.GetTheSubscriptionKey();
            string serverLocation = Handler_Data.GetTheServerLocation();
            config = SpeechConfig.FromSubscription(subscriptionKey, serverLocation);
            // Initialize your synthesizer and other components
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
        public static async Task SynthesizeAudioAsync(string soundfile, string formatOutputSound, bool _audioOn)
        {
            #region Producing the sound data
            XmlDocument xmlDoc = new();
            xmlDoc.Load(soundfile);
            string ssmlText = xmlDoc.OuterXml;
            SoundPause();
            if (!_audioOn) { speechSynthesizer = new SpeechSynthesizer(config, null); } // Note : SpeechSynthesizer(speechConfig, null) gets a result as an in-memory stream
            else { speechSynthesizer = new SpeechSynthesizer(config, null); }
            speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(ssmlText);
            //SpeechSynthesisResult speechSynthesisResult = await speechSynthesizer.SpeakSsmlAsync(ssmlText);
            #endregion
            #region Saving the sound data to the disk as a specific sound format
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            using Stream stream = new MemoryStream(speechSynthesisResult.AudioData);
            if (formatOutputSound != "None" || formatOutputSound != null)
            {
                switch (formatOutputSound)
                {
                    case "mp3":
                        //outputFile = Path.ChangeExtension(outputFile, "."+formatOutputSound);
                        MediaFoundationApi.Startup();
                        var reader = new WaveFileReader(stream);  // Normally public WaveFileReader(Stream inputStream) ought to handle it properly but it does not accept it
                        MediaFoundationEncoder.EncodeToMp3(reader, outputFile);
                        break;

                    case "wav":
                        MediaFoundationApi.Startup();
                        var reader1 = new WaveFileReader(stream);
                        WaveFileWriter.CreateWaveFile(outputFile, reader1);
                        break;

                    case "ogg": // add an ogg vorbis encoder 
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }
        public static async Task SynthesizeAudioAsyncFromText(XmlDocument _xmlDoc, string soundfile, string formatOutputSound, bool _audioOn)
        {
            #region Producing the sound data
            //XmlDocument xmlDoc = new();
            //_xmlDoc.LoadXml(soundfile);
            string ssmlText = _xmlDoc.OuterXml;
            SoundPause();
            if (!_audioOn) { speechSynthesizer = new SpeechSynthesizer(config, null); } // Note : SpeechSynthesizer(speechConfig, null) gets a result as an in-memory stream
            else { speechSynthesizer = new SpeechSynthesizer(config, null); }
            SpeechSynthesisResult result = await speechSynthesizer.SpeakSsmlAsync(ssmlText);
            #endregion
            #region Saving the sound data to the disk as a specific sound format
            string outputFile = Path.ChangeExtension(soundfile, "." + formatOutputSound);
            using Stream stream = new MemoryStream(result.AudioData);
            if (formatOutputSound != "None" || formatOutputSound != null)
            {
                switch (formatOutputSound)
                {
                    case "mp3":
                        //outputFile = Path.ChangeExtension(outputFile, "."+formatOutputSound);
                        MediaFoundationApi.Startup();
                        var reader = new WaveFileReader(stream);  // Normally public WaveFileReader(Stream inputStream) ought to handle it properly but it does not accept it
                        MediaFoundationEncoder.EncodeToMp3(reader, outputFile);
                        break;

                    case "wav":
                        MediaFoundationApi.Startup();
                        var reader1 = new WaveFileReader(stream);
                        WaveFileWriter.CreateWaveFile(outputFile, reader1);
                        break;

                    case "ogg": // add an ogg vorbis encoder 
                        break;
                    default:
                        break;
                }
            }
            #endregion
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
