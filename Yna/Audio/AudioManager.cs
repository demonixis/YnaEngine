using System;
#if !NETFX_CORE
using System.IO;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Yna;
using Yna.Content;

namespace Yna.Audio
{
    /// <summary>
    /// An audio manager who allow you to play musics and sounds from XNA's Content Manager, or
    /// from a custom Content Manager who doen't required XNB, but wave file or wma files.
    /// Warning : For now the custom content manager isn't working on Windows 8 Metro.
    /// </summary>
    public class AudioManager
    {
        #region Private declarations

        private bool _soundEnabled;
        private float _soundVolume;
        private bool _musicEnabled;

#if MONOGAME && WINDOWS
        private WMPLib.WindowsMediaPlayer _windowMediaPlayer;
#endif

        #endregion

        #region Properties

        /// <summary>
        /// Active or desactive sounds
        /// </summary>
        public bool SoundEnabled
        {
            get { return _soundEnabled; }
            set { _soundEnabled = value; }
        }

        /// <summary>
        /// Get or set the sound volume
        /// </summary>
        public float SoundVolume
        {
            get { return _soundVolume; }
            set { _soundVolume = value; }
        }

        /// <summary>
        /// Active or desactive music
        /// </summary>
        public bool MusicEnabled
        {
            get { return _musicEnabled; }
            set { _musicEnabled = value; }
        }

        /// <summary>
        /// Get or Set the music volume
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = Math.Abs(value); }
        }

        #endregion

        #region Singleton instance & constructor

        public AudioManager()
        {
            MusicEnabled = true;
            SoundEnabled = true;
            SoundVolume = 0.4f;
            MusicVolume = 0.6f;

#if MONOGAME && WINDOWS
            _windowMediaPlayer = new WMPLib.WindowsMediaPlayer();
#endif
        }

        #endregion

        #region Playing sound from XNA's Content Manager

        private void PlaySound(SoundEffect sound, float volume, float pitch, float pan)
        {
            if (volume > _soundVolume)
                volume = _soundVolume;
            
            sound.Play(volume, pitch, pan);
        }

        public void PlaySound(string assetName, float volume, float pitch, float pan)
        {
            if (_soundEnabled)
            {
                SoundEffect sound = YnG.Content.Load<SoundEffect>(assetName);
                PlaySound(sound, volume, pitch, pan);
            }
        }

        #endregion

        #region Playing music from XNA's Content Manager

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="repeat"></param>
        public void PlayMusic(string assetName, bool repeat)
        {
            if (_musicEnabled)
            {
#if XNA || NETFX_CORE || WINDOWS_PHONE_7
                Song music = YnG.Content.Load<Song>(assetName);
                PlayMusic(music, repeat);
#elif MONOGAME && WINDOWS
                if (_windowMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    _windowMediaPlayer.controls.stop();
                
                _windowMediaPlayer.URL = YnG.Content.RootDirectory + "/" + assetName + ".wma";

                _windowMediaPlayer.controls.play();
#endif
            }
        }

        /// <summary>
        /// Play a song music
        /// </summary>
        /// <param name="music"></param>
        /// <param name="repeat"></param>
        private void PlayMusic(Song music, bool repeat)
        {
            StopMusic();

            MediaPlayer.IsRepeating = repeat;
            MediaPlayer.Play(music);
        }

        #endregion

        #region Music controls

        /// <summary>
        /// Stop the current music
        /// </summary>
        public void StopMusic()
        {
            if (MediaPlayer.State != MediaState.Stopped)
                MediaPlayer.Stop();
        }

        /// <summary>
        /// Pause the current music
        /// </summary>
        public void PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        public void ResumeMusic()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        #endregion

        public void Dispose()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();

#if MONOGAME && WINDOWS
            if (_windowMediaPlayer.playState != WMPLib.WMPPlayState.wmppsStopped)
                _windowMediaPlayer.controls.stop();

            _windowMediaPlayer = null;
#endif
        }
    }
}
