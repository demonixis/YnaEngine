using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Yna.Framework.Audio
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

        /// <summary>
        /// Play a music from the XNA's content manager
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="repeat"></param>
        public override void PlayMusic(string assetName)
        {
            if (_musicEnabled)
            {
#if !LINUX && !MACOSX
                Song music = YnG.Content.Load<Song>(assetName);
                // PlayMusic(music, repeat); // FIXME : repeat attribute is missing ?!
#endif
            }
        }

		public override void PlayMusic (string assetName, bool repeat)
		{
			_repeatMusic = repeat;
			PlayMusic(assetName);
		}

        /// <summary>
        /// Play a song music
        /// </summary>
        /// <param name="music"></param>
        /// <param name="repeat"></param>
        public void PlayMusic(Song music, bool repeat)
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

		#region Sound

        public override void PlaySound(string path)
        {
			PlaySound(path, 1.0f);
        }

        public override void PlaySound(string path, float volume)
        {
			PlaySound(path, volume, 1.0f, 0.0f);
        }

        public override void PlaySound(string path, float volume, float pitch, float pan)
        {
            if (_soundEnabled)
            {
                SoundEffect sound = YnG.Content.Load<SoundEffect>(path);
				sound.Play(volume, pitch, pan);
            }
        }

		#endregion

		#region IDisposable implementation
		public override void Dispose()
		{
			MediaPlayer.Stop();
		}
		#endregion
    }
}
