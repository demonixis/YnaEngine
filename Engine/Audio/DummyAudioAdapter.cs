// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Audio;

namespace Yna.Engine.Audio
{
    /// <summary>
    /// Dummy audio adapter. This adapter will not play sound or music, it's used for
    /// MonoGame version that doesn't compatible with audio.
    /// </summary>
    public class DummyAudioAdapter : AudioAdapter
    {
        public new AudioState AudioState
        {
            get { return AudioState.Stopped; }
        }

        public override void PlaySound(string path, float volume, float pitch, float pan)
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
