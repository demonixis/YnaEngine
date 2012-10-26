using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Yna;
using Yna.Content;

namespace Yna.Audio
{
    public class AudioManager
    {
        public static readonly object _object = new object();
        public static AudioManager _instance = null;

        private bool _soundEnabled;
        private float _soundVolume;
        private bool _musicEnabled;

        #region Properties

        public bool SoundEnabled
        {
            get { return _soundEnabled; }
            set { _soundEnabled = value; }
        }

        public float SoundVolume
        {
            get { return _soundVolume; }
            set { _soundVolume = value; }
        }

        public bool MusicEnabled
        {
            get { return _musicEnabled; }
            set { _musicEnabled = value; }
        }

        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        #endregion

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

        private void CheckYnContent()
        {
            if (YnG.YnContent == null)
                YnG.YnContent = new YnContent();
        }

        public void PlaySound(string assetName, float volume, float pitch = 0.0f, float pan = 0.0f)
        {
            if (_soundEnabled)
            {
                SoundEffect sound = YnG.Content.Load<SoundEffect>(assetName);
                PlaySound(sound, pitch, pan);
            }
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

        private void PlaySound(SoundEffect sound, float volume, float pitch = 0.0f, float pan = 0.0f)
        {
            if (volume > _soundVolume)
                    volume = _soundVolume;

            sound.Play(volume, pitch, pan);
        }

        // TODO : Add a custom content, remove the name parameter (determine it)
        private Song LoadMusicFromStream(string name, string path)
        {
            CheckYnContent();
            return YnG.YnContent.Load<Song>(path);
        }

        /// <summary>
        /// Load a sound from a stream and stored automatically it in a custom content manager
        /// see : YnContent
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private SoundEffect LoadSoundFromStream(string path)
        {
            CheckYnContent();
            return YnG.YnContent.Load<SoundEffect>(path);
        }

        // TODO : Add a custom content, remove the name parameter (determine it)
        public void PlayMusicFromStream(string name, string path, bool repeat)
        {
            if (_musicEnabled)
            {
                Song music = LoadMusicFromStream(name, path);

                if (music != null)
                    PlayMusic(music, repeat);
            }
        }

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

        public void Dispose()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }
    }
}
