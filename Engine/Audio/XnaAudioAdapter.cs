// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Yna.Engine.Audio
{
    /// <summary>
    /// Xna audio adapter.
    /// </summary>
    public class XnaAudioAdapter : AudioAdapter
    {
        public new AudioState AudioState
        {
            get
            {
                AudioState state = AudioState.Stopped;

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

        #region Music management methods

        public override void PlayMusic(string assetName, bool repeat)
        {
            _repeatMusic = repeat;
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
        public override void StopMusic()
        {
            if (MediaPlayer.State != MediaState.Stopped)
                MediaPlayer.Stop();
        }

        /// <summary>
        /// Pause the current music
        /// </summary>
        public override void PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        public override void ResumeMusic()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        #endregion

        #region Sound management

        public override void PlaySound(string path, float volume, float pitch, float pan)
        {
            if (_soundEnabled)
            {
                SoundEffect sound = YnG.Content.Load<SoundEffect>(path);
                sound.Play(volume, pitch, pan);
            }
        }

        #endregion

        public override void Dispose()
        {
            MediaPlayer.Stop();
        }
    }
}
