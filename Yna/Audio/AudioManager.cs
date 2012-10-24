using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Yna;

namespace SpaceGame.Manager
{
    public class AudioManager
    {
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

        public AudioManager()
        {
            MusicEnabled = true;
            SoundEnabled = true;
            SoundVolume = 0.4f;
            MusicVolume = 0.6f;
        }

        public void PlaySound(string assetName, float volume, float pitch = 1.0f, float pan = 0.0f)
        {
            if (_soundEnabled)
            {
                if (volume > _soundVolume)
                    volume = _soundVolume;

                SoundEffect sound = YnG.Content.Load<SoundEffect>(assetName);
                sound.Play(volume, pitch, pan);
            }
        }

        public void PlayMusic(string assetName, bool repeat = true)
        {
            if (MusicEnabled)
            {
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Stop();

                Song music = YnG.Content.Load<Song>(assetName);
                MediaPlayer.IsRepeating = repeat;
                MediaPlayer.Play(music);
            }
        }

        public void StopMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }

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
