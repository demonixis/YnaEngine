using System;
#if !NETFX_CORE
using System.IO;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Yna.Framework;

namespace Yna.Framework.Audio
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
			_audioAdapter = new XnaAudioAdapter();
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

        public void PlaySound(string assetName, float volume, float pitch, float pan)
        {
			_audioAdapter.PlaySound(assetName, volume, pitch, pan);
        }

		#region Music controls

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="repeat"></param>
        public void PlayMusic(string assetName, bool repeat)
        {
			_audioAdapter.PlayMusic(assetName, repeat);
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
