// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Yna.Engine.Audio
{
    /// <summary>
    /// Xna audio adapter.
    /// </summary>
    public class SDLMixerAdapter : AudioAdapter
    {
        private Dictionary<string, IntPtr> musicsCollection;
        private Dictionary<string, IntPtr> soundsCollection;
        private IntPtr _activeMusic;

        public new AudioState AudioState
        {
            get
            {
                AudioState state = AudioState.Stopped;

                if (SDL2.SDL_mixer.Mix_PausedMusic() > 0)
                    state = AudioState.Paused;
                else if (SDL2.SDL_mixer.Mix_PlayingMusic() > 0)
                    state = AudioState.Playing;

                return state;
            }
        }

        public SDLMixerAdapter()
            : base()
        {
            SDL2.SDL_mixer.Mix_OpenAudio(44100, SDL2.SDL_mixer.MIX_DEFAULT_FORMAT, SDL2.SDL_mixer.MIX_DEFAULT_CHANNELS, 1024);
            musicsCollection = new Dictionary<string, IntPtr>();
            soundsCollection = new Dictionary<string, IntPtr>();
        }

        public override void PlayMusic(string assetName, bool repeat)
        {
            IntPtr musicId = IntPtr.Zero;

            string contentPath = String.Format("{0}/{1}.ogg", new object[] { YnG.Content.RootDirectory, assetName });

            if (musicsCollection.ContainsKey(contentPath))
                musicId = musicsCollection[contentPath];
            else
            {
                musicId = SDL2.SDL_mixer.Mix_LoadMUS(contentPath);
                musicsCollection.Add(contentPath, musicId);
            }

            if (_musicEnabled)
                SDL2.SDL_mixer.Mix_PlayMusic(musicId, repeat ? 0 : -1);
        }

        /// <summary>
        /// Play a song music
        /// </summary>
        /// <param name="music"></param>
        /// <param name="repeat"></param>
        private void PlayMusic(Song music, bool repeat)
        {
            
        }

        /// <summary>
        /// Stop the current music
        /// </summary>
        public override void StopMusic()
        {
            SDL2.SDL_mixer.Mix_HaltMusic();
        }

        /// <summary>
        /// Pause the current music
        /// </summary>
        public override void PauseMusic()
        {
            SDL2.SDL_mixer.Mix_PausedMusic();
        }

        /// <summary>
        /// Resume the current music
        /// </summary>
        public override void ResumeMusic()
        {
            SDL2.SDL_mixer.Mix_ResumeMusic();
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="path"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <param name="pan"></param>
        public override void PlaySound(string path, float volume, float pitch, float pan)
        {
            IntPtr soundId = IntPtr.Zero;

            string contentPath = String.Format("{0}/{1}.wav", new object[] { YnG.Content.RootDirectory, path });

            if (soundsCollection.ContainsKey(contentPath))
                soundId = soundsCollection[contentPath];
            else
            {
                soundId = SDL2.SDL_mixer.Mix_LoadWAV(contentPath);
                soundsCollection.Add(contentPath, soundId);
            }

            if (_soundEnabled && soundId != IntPtr.Zero)
            {
                SDL2.SDL_mixer.Mix_VolumeChunk(soundId, (int)(volume * 128));
                SDL2.SDL_mixer.Mix_PlayChannel(-1, soundId, 0);
            }
        }

        public override void Dispose()
        {
            if (musicsCollection.Count > 0)
            {
                foreach (KeyValuePair<string, IntPtr> keyValue in musicsCollection)
                    SDL2.SDL_mixer.Mix_FreeMusic(keyValue.Value);
            }

            if (soundsCollection.Count > 0)
            {
                foreach (KeyValuePair<string, IntPtr> keyValue in soundsCollection)
                    SDL2.SDL_mixer.Mix_FreeChunk(keyValue.Value);
            }

            SDL2.SDL_mixer.Mix_CloseAudio();
        }
    }
}

