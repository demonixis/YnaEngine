// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Audio
{
    public enum AudioState
    {
        Playing = 0, Stopped, Paused
    }

	/// <summary>
	/// A base class for make an audio adapter who is used to play sound and music
	/// </summary>
    public abstract class AudioAdapter : IDisposable
    {
        protected bool _soundEnabled;
        protected bool _musicEnabled;
        protected float _musicVolume;
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

        public bool RepeatMusic
        {
            get { return _repeatMusic; }
            set { _repeatMusic = value; }
        }

        public AudioState AudioState
        {
            get { return _audioState; }
        }

        #endregion

        public AudioAdapter()
        {
            _musicEnabled = true;
            _soundEnabled = true;
            _musicVolume = 1.0f;
            _repeatMusic = false;
            _audioState = AudioState.Stopped;
        }

        // Music
		public abstract void PlayMusic(string path, bool repeat);
        public abstract void StopMusic();
        public abstract void PauseMusic();
        public abstract void ResumeMusic();

        // Sound
        public abstract void PlaySound(string path, float volume, float pitch, float pan);

		public abstract void Dispose();
    }
}
