﻿using System;
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

        public static readonly object _object = new object();
        public static AudioManager _instance = null;

        private bool _soundEnabled;
        private float _soundVolume;
        private bool _musicEnabled;

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
            set { MediaPlayer.Volume = value; }
        }

        #endregion

        #region Singleton instance & constructor

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_object)
                    {
                        if (_instance == null)
                            _instance = new AudioManager();
                    }
                }

                return _instance;
            }
        }

        private AudioManager()
        {
            MusicEnabled = true;
            SoundEnabled = true;
            SoundVolume = 0.4f;
            MusicVolume = 0.6f;
        }

        #endregion

#if !NETFX_CORE
        private void CheckYnContent()
        {
            if (YnG.YnContent == null)
                YnG.YnContent = new YnContent();
        }

        #region Playing music from custom Content Manager

        private Song LoadMusicFromStream(string name, string path)
        {
            CheckYnContent();
            return YnG.YnContent.Load<Song>(path);
        }

        public void PlayMusicFromStream(string path, bool repeat)
        {
            if (_musicEnabled)
            {
                Song music = YnG.YnContent.Load<Song>(path);

                if (music != null)
                    PlayMusic(music, repeat);
            }
        }

        #endregion

        #region Playing sound from custom Content Manager

        /// <summary>
        /// Load a sound from a stream and stored automatically it in a custom content manager
        /// see : YnContent
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public SoundEffect LoadSoundFromStream(string path)
        {
            CheckYnContent();
            return YnG.YnContent.Load<SoundEffect>(path);
        }

        public void PlaySoundFromStream(string path, float volume, float pitch = 0.0f, float pan = 0.0f)
        {
            if (_soundEnabled)
            {
                SoundEffect sound = LoadSoundFromStream(path);

                if (sound != null)
                    PlaySound(sound, volume, pitch, pan);
            }
        }

        #endregion
#endif

        #region Playing sound from XNA's Content Manager

        private void PlaySound(SoundEffect sound, float volume, float pitch = 0.0f, float pan = 0.0f)
        {
            if (volume > _soundVolume)
                volume = _soundVolume;

            sound.Play(volume, pitch, pan);
        }

        public void PlaySound(string assetName, float volume, float pitch = 0.0f, float pan = 0.0f)
        {
            if (_soundEnabled)
            {
                SoundEffect sound = YnG.Content.Load<SoundEffect>(assetName);
                PlaySound(sound, pitch, pan);
            }
        }

        #endregion

        #region Playing music from XNA's Content Manager

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="repeat"></param>
        public void PlayMusic(string assetName, bool repeat = true)
        {
            if (_musicEnabled)
            {
                Song music = YnG.Content.Load<Song>(assetName);
                PlayMusic(music, repeat);
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
        }
    }
}
