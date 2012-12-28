using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Globalization;

namespace Yna.Framework.Audio
{
    public class SpeechSynthesis : IDisposable
    {
        private SpeechSynthesizer _speechSynth;
        private List<string> _voices;
        private string _culture;
        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }

        public int Rate
        {
            get { return _speechSynth.Rate; }
            set { _speechSynth.Rate = value; }
        }

        public SpeechSynthesis()
        {
            _speechSynth = new SpeechSynthesizer();
            _speechSynth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(speechSynth_SpeakCompleted);
            _voices = new List<string>();
            _speechSynth.Rate = 2;
            _culture = "fr-FR";
            _enabled = true;
        }

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

        public void SpeakAsync(PromptBuilder builder)
        {
            if (_enabled && (_voices.Count > 0))
            {
                StopSpeak();
                _speechSynth.SpeakAsync(builder);
            }
        }

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