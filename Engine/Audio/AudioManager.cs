// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Audio
{
    /// <summary>
    /// An audio manager for playing musics and sounds
    /// </summary>
    public class AudioManager : IDisposable
    {
        protected AudioAdapter _audioAdapter;

        #region Properties

        /// <summary>
        /// Active or desactive sounds
        /// </summary>
        public bool SoundEnabled
        {
            get { return _audioAdapter.SoundEnabled; }
            set { _audioAdapter.SoundEnabled = value; }
        }

        /// <summary>
        /// Active or desactive music
        /// </summary>
        public bool MusicEnabled
        {
            get { return _audioAdapter.MusicEnabled; }
            set { _audioAdapter.MusicEnabled = value; }
        }

        /// <summary>
        /// Get or Set the music volume
        /// </summary>
        public float MusicVolume
        {
            get { return _audioAdapter.MusicVolume; }
            set { _audioAdapter.MusicVolume = value; }
        }

		/// <summary>
		/// Gets the audio adapter.
		/// </summary>
		public AudioAdapter AudioAdapter
		{
			get { return _audioAdapter; }
		}

        #endregion

		/// <summary>
		/// Create an audio manager object with default audio adapter.
		/// </summary>
        public AudioManager()
        {
#if SDL2
            _audioAdapter = new SDLMixerAdapter();
#else
			_audioAdapter = new XnaAudioAdapter();
#endif
            _audioAdapter.MusicEnabled = true;
            _audioAdapter.SoundEnabled = true;
            _audioAdapter.MusicVolume = 0.6f;
			_audioAdapter.RepeatMusic = false;
        }

		/// <summary>
		/// Create an audio manager object with a custom audio adapter.
		/// </summary>
		/// <param name='audioAdapter'>A custom audio adapter.</param>
		public AudioManager(AudioAdapter audioAdapter)
		{
			_audioAdapter = audioAdapter;
		}

        #region Sound control

        /// <summary>
        /// Play a sound.
        /// </summary>
        /// <param name="assetName">Name of the sound</param>
        /// <param name="volume">Volume value between 1.0f and 0.0f.</param>
        /// <param name="pitch">Pitch value between 1.0f and 0.0f.</param>
        /// <param name="pan">Pan value between 1.0f and -1.0f</param>
        public void PlaySound(string assetName, float volume, float pitch, float pan)
        {
			_audioAdapter.PlaySound(assetName, volume, pitch, pan);
        }

        /// <summary>
        /// Play a sound.
        /// </summary>
        /// <param name="assetName">Name of the sound</param>
        /// <param name="volume">Volume value between 1.0f and 0.0f.</param>
        public void PlaySound(string assetName, float volume)
        {
            PlaySound(assetName, volume, 1.0f, 0.0f);
        }

        /// <summary>
        /// Play a sound.
        /// </summary>
        /// <param name="assetName">Name of the sound</param>
        public void PlaySound(string assetName)
        {
            PlaySound(assetName, 1.0f, 1.0f, 0.0f);
        }

        #endregion

        #region Music controls

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName">Name of the music to play.</param>
        /// <param name="repeat">Enable or disable repeat</param>
        public void PlayMusic(string assetName, bool repeat)
        {
			_audioAdapter.PlayMusic(assetName, repeat);
        }

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName">Name of the music to play.</param>
        public void PlayMusic(string assetName)
        {
            _audioAdapter.PlayMusic(assetName, false);
        }

        /// <summary>
        /// Stop the current music
        /// </summary>
        public void StopMusic()
        {
			_audioAdapter.StopMusic();
        }

        /// <summary>
        /// Pause the current music
        /// </summary>
        public void PauseMusic()
        {
            _audioAdapter.PauseMusic();
        }

        public void ResumeMusic()
        {
            _audioAdapter.ResumeMusic();
        }

        #endregion

        public void Dispose()
        {
			_audioAdapter.Dispose();
        }
    }
}
