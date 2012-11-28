using System;

namespace Yna.Audio
{
    public interface IAudioAdapter : IDisposable
    {
        void PlayMusic(string path);
        void StopMusic();
        void PauseMusic();
        void ResumeMusic();
        void PlaySound(string path);
    }
}
