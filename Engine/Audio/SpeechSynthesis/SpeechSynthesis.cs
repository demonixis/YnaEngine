using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Globalization;

namespace Yna.Engine.Audio
{
    /// <summary>
    /// A class to use native speech synthesizer with ease
    /// </summary>
    public class SpeechSynthesis : IDisposable
    {
        private SpeechSynthesizer _speechSynth;
        private List<string> _voices;
        private string _culture;
        private bool _enabled;

        /// <summary>
        /// Enable or disable speech.
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Gets or sets the culture to use.
        /// </summary>
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }

        /// <summary>
        /// Define the rate of the voice.
        /// </summary>
        public int Rate
        {
            get { return _speechSynth.Rate; }
            set { _speechSynth.Rate = value; }
        }

        /// <summary>
        /// Gets available voices.
        /// </summary>
        public string[] AvailableVoices
        {
            get { return _voices.ToArray(); }
        }

        /// <summary>
        /// Create a speech synthesis.
        /// </summary>
        /// <param name="lang">Lang code to use.</param>
        public SpeechSynthesis(string lang)
        {
            _speechSynth = new SpeechSynthesizer();
            _speechSynth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(speechSynth_SpeakCompleted);
            _voices = new List<string>();
            _speechSynth.Rate = 2;
            _culture = lang;
            _enabled = true;
        }

        /// <summary>
        /// Create a speech synthesis with english voice.
        /// </summary>
        public SpeechSynthesis()
            : this("en-US")
        {
        }

        /// <summary>
        /// Initialize the synthesizer and available voices.
        /// </summary>
        public void Initialize()
        {
            var installedVoices = _speechSynth.GetInstalledVoices();

            if (installedVoices.Count > 0)
            {
                foreach (InstalledVoice v in installedVoices)
                    _voices.Add(v.VoiceInfo.Name);
            }
            else
                throw new Exception("[SpeechSynthesis] No voices founded on this computer");
        }

        /// <summary>
        /// Speaks asynchronously
        /// </summary>
        /// <param name="message">Message to speak</param>
        /// <param name="sayAs">Type of pronunciation</param>
        public void SpeakAsync(string message, SayAs sayAs = SayAs.Text)
        {
            if (_enabled && (_voices.Count > 0))
            {
                StopSpeak();

                PromptBuilder builder = null;
                try
                {
                    builder = new PromptBuilder(CultureInfo.CreateSpecificCulture(_culture));
                }
                catch (Exception e)
                {
                    builder = new PromptBuilder();
                }

                builder.AppendTextWithHint(message, sayAs);

                _speechSynth.SpeakAsync(builder);
            }
        }

        /// <summary>
        /// Speaks asynchronously.
        /// </summary>
        /// <param name="builder">A prompt builder with custom paramaters</param>
        public void SpeakAsync(PromptBuilder builder)
        {
            if (_enabled && (_voices.Count > 0))
            {
                StopSpeak();
                _speechSynth.SpeakAsync(builder);
            }
        }

        /// <summary>
        /// Speaks synchronously, be award because you program will be blocked during the talk.
        /// </summary>
        /// <param name="message">Message to speak.</param>
        /// <param name="sayAs">Type of pronunciation</param>
        public void Speak(string message, SayAs sayAs = SayAs.Text)
        {
            if (_enabled && (_voices.Count > 0))
            {
                StopSpeak();

                PromptBuilder builder = new PromptBuilder(CultureInfo.CreateSpecificCulture(_culture));
                builder.AppendTextWithHint(message, sayAs);

                _speechSynth.Speak(builder);
            }
        }

        /// <summary>
        /// Stop speaking.
        /// </summary>
        /// <returns>Return true if the synthesizer speaked and stopped, otherwise return false.</returns>
        public bool StopSpeak()
        {
            if (_speechSynth.State == SynthesizerState.Speaking)
            {
                Prompt prompt = _speechSynth.GetCurrentlySpokenPrompt();

                if (prompt != null)
                {
                    _speechSynth.SpeakAsyncCancel(prompt);
                    return true;
                }
            }

            return false;
        }

        private void speechSynth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (e.Error != null)
                Console.Error.WriteLine(e.Error.Message);
        }

        /// <summary>
        /// Close the synthesizer and dispose it.
        /// </summary>
        public void Close()
        {
            if (_speechSynth.State == SynthesizerState.Speaking)
                _speechSynth.SpeakAsyncCancelAll();

            _speechSynth.Dispose();
        }

        void IDisposable.Dispose()
        {
            Close();
        }
    }
}