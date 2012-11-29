using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Yna.Audio
{
    public class XnaAudioAdapter : AudioAdapter, IAudioAdapter
    {
        public new AudioState AudioState
        {
            get 
            {
                switch (MediaPlayer.State)
                {
                    case MediaState.Paused: _audioState = Audio.AudioState.Paused; break;
                    case MediaState.Playing: _audioState = Audio.AudioState.Playing; break;
                    case MediaState.Stopped: _audioState = Audio.AudioState.Stopped; break;
                }

                return _audioState;
            }
            protected set { _audioState = value; }
        }

        public new float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                if (value < 0)
                    _musicVolume = 0;
                else if (value > 1)
                    _musicVolume = 1;
                else
                    _musicVolume = value;

                MediaPlayer.Volume = _musicVolume;
            }
        }

        public XnaAudioAdapter()
            : base()
        {

        }

        void IAudioAdapter.PlayMusic(string path)
        {
            Song music = YnG.Content.Load<Song>(path);
            MediaPlayer.Play(music);
        }

        void IAudioAdapter.StopMusic()
        {
            if (MediaPlayer.State != MediaState.Stopped)
                MediaPlayer.Stop();
        }

        void IAudioAdapter.PauseMusic()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
        }

        void IAudioAdapter.ResumeMusic()
        {
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
        }

        void IAudioAdapter.PlaySound(string path)
        {
            (this as IAudioAdapter).PlaySound(path, _soundVolume, _pitch, _pan);
        }

        void IAudioAdapter.PlaySound(string path, float volume, float pitch, float pan)
        {
            SoundEffect sound = YnG.Content.Load<SoundEffect>(path);
            sound.Play(volume, pitch, pan);
        }

        void System.IDisposable.Dispose()
        {
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();
        }
    }
}
