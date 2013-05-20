using Microsoft.Xna.Framework.Audio;

namespace Yna.Engine.Audio
{
    /// <summary>
    /// Xna audio adapter.
    /// </summary>
    public class DummyAudioAdapter : AudioAdapter
    {
        public new AudioState AudioState
        {
            get { return AudioState.Stopped; }
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

        public override void PlayMusic(string path)
        {

        }

        public override void PlayMusic(string path, bool repeat)
        {

        }

        public override void StopMusic()
        {

        }

        public override void PauseMusic()
        {

        }

        public override void ResumeMusic()
        {

        }

        public override void Dispose()
        {

        }
    }
}
