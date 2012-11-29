using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Audio
{
    public class AudioEngine
    {
        protected IAudioAdapter _audioAdapter;
        protected AudioAdapter _adapter;

        #region Properties

        public bool MusicEnabled
        {
            get { return _adapter.MusicEnabled; }
            set { _adapter.MusicEnabled = value; }
        }

        public bool SoundEnabled
        {
            get { return _adapter.SoundEnabled; }
            set { _adapter.SoundEnabled = value; }
        }

        public float SoundVolume
        {
            get { return _adapter.SoundVolume; }
            set { _adapter.SoundVolume = value; }
        }

        public float MusicVolume
        {
            get { return _adapter.MusicVolume; }
            set { _adapter.MusicVolume = value; }
        }

        public float Pitch
        {
            get { return _adapter.Pitch; }
            set { _adapter.Pitch = value; }
        }

        public float Pan
        {
            get { return _adapter.Pan; }
            set { _adapter.Pan = value; }
        }

        public bool RepeatMusic
        {
            get { return _adapter.RepeatMusic; }
            set { _adapter.RepeatMusic = value; }
        }

        public AudioState AudioState
        {
            get { return _adapter.AudioState; }
        }

        #endregion

        public AudioEngine(IAudioAdapter adapter)
        {
            _audioAdapter = adapter;
            _adapter = (_audioAdapter as AudioAdapter);
        }

        public void PlaySound(string path)
        {
            if (SoundEnabled)
                _audioAdapter.PlaySound(path);
        }

        public void PlayMusic(string path)
        {
            if (MusicEnabled)
                _audioAdapter.PlayMusic(path);
        }

        public void StopMusic()
        {
            _audioAdapter.StopMusic();
        }

        public void PauseMusic()
        {
            _audioAdapter.PauseMusic();
        }
    }
}
