// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace Yna.Engine.Audio
{
    public enum AudioState
    {
        Playing = 0, Stopped, Paused
    }

    /// <summary>
    /// An audio manager for playing musics and sounds
    /// </summary>
    public class AudioManager : IDisposable
    {
        #region Properties

        /// <summary>
        /// Active or desactive sounds
        /// </summary>
        public bool SoundEnabled { get; set; } = true;

        /// <summary>
        /// Active or desactive music
        /// </summary>
        public bool MusicEnabled { get; set; } = true;

        /// <summary>
        /// Get or Set the music volume
        /// </summary>
        public float MusicVolume { get; set; } = 1.0f;

        public AudioState AudioState
        {
            get
            {
                var state = AudioState.Stopped;

                switch (MediaPlayer.State)
                {
                    case MediaState.Playing:
                        state = AudioState.Playing;
                        break;
                    case MediaState.Paused:
                        state = AudioState.Paused;
                        break;
                    case MediaState.Stopped:
                        state = AudioState.Stopped;
                        break;
                }

                return state;
            }
        }


        #endregion

        #region Sound control

        /// <summary>
        /// Play a sound.
        /// </summary>
        /// <param name="assetName">Name of the sound</param>
        /// <param name="volume">Volume value between 1.0f and 0.0f.</param>
        /// <param name="pitch">Pitch value between 1.0f and 0.0f.</param>
        /// <param name="pan">Pan value between 1.0f and -1.0f</param>
        public void PlaySound(string assetName, float volume = 1.0f, float pitch = 1.0f, float pan = 0.0f)
        {
            if (!SoundEnabled)
                return;

            var sound = YnG.Content.Load<SoundEffect>(assetName);
            sound.Play(volume, pitch, pan);
        }

        #endregion

        #region Music controls

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName">Name of the music to play.</param>
        /// <param name="repeat">Enable or disable repeat</param>
        public void PlayMusic(string assetName, bool repeat = true)
        {
            StopMusic();

            var music = YnG.Content.Load<Song>(assetName);
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

        #endregion

        public void Dispose() => MediaPlayer.Stop();
    }
}
