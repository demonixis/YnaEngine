using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Audio
{
    public enum AudioState
    {
        Playing = 0, Stopped, Paused
    }

    public class AudioAdapter
    {
        protected bool _soundEnabled;
        protected bool _musicEnabled;
        protected float _pitch;
        protected float _pan;
        protected float _musicVolume;
        protected float _soundVolume;
        protected bool _repeatMusic;
        protected AudioState _audioState;

        #region Properties

        public bool MusicEnabled
        {
            get { return _musicEnabled; }
            set { _musicEnabled = value; }
        }

        public bool SoundEnabled
        {
            get { return _soundEnabled; }
            set { _soundEnabled = value; }
        }

        public float SoundVolume
        {
            get { return _soundVolume; }
            set
            {
                if (value < 0)
                    _soundVolume = 0;
                else if (value > 1)
                    _soundVolume = 1;
                else
                    _soundVolume = value;
            }
        }

        public float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                if (value < 0)
                    _musicVolume = 0;
                else if (value > 1)
                    _musicVolume = 1;
                else
                    _musicVolume = value;
            }
        }

        public float Pitch
        {
            get { return _pitch; }
            set
            {
                if (value < 0)
                    _pitch = 0;
                else if (value > 1)
                    _pitch = 1;
                else
                    _pitch = value;
            }
        }

        public float Pan
        {
            get { return _pan; }
            set
            {
                if (value < -1.0f)
                    _pan = -1.0f;
                else if (value > 1.0f)
                    _pan = 1.0f;
                else
                    _pan = value;
            }
        }

        public bool RepeatMusic
        {
            get { return _repeatMusic; }
            set { _repeatMusic = value; }
        }

        public AudioState AudioState
        {
            get { return _audioState; }
            protected set { _audioState = value; }
        }

        #endregion

        public AudioAdapter()
        {
            _musicEnabled = true;
            _soundEnabled = true;
            _musicVolume = 1.0f;
            _soundVolume = 1.0f;
            _pan = 0.0f;
            _pitch = 1.0f;
            _repeatMusic = false;
            _audioState = AudioState.Stopped;
        }
    }
}
