using System;
using NAudio;
using NAudio.Wave;

namespace Yna.Audio
{
    public class NAudioAdapter : AudioAdapter, IAudioAdapter
    {
        private IWavePlayer _wavePlayer;
        private WaveStream _mainOutputStream;
        private WaveStream _volumeSytream;

        public new AudioState AudioState
        {
            get
            {
                if (_wavePlayer == null)
                {
                    _audioState = AudioState.Stopped;
                }
                else
                {
                    switch (_wavePlayer.PlaybackState)
                    {
                        case PlaybackState.Paused: _audioState = Audio.AudioState.Paused; break;
                        case PlaybackState.Playing: _audioState = Audio.AudioState.Playing; break;
                        case PlaybackState.Stopped: _audioState = Audio.AudioState.Stopped; break;
                    }
                }

                return _audioState;
            }
            protected set { _audioState = value; }
        }

        public NAudioAdapter()
            : base()
        {
            _wavePlayer = new WaveOut();
            _mainOutputStream = null;
            _volumeSytream = null;
        }

        private WaveStream CreateInputStream(string filename)
        {
            WaveStream inputStream;

            if (filename.EndsWith(".mp3"))
            {
                WaveStream mp3Reader = new Mp3FileReader(filename);

                if (_repeatMusic)
                    inputStream = new NAudioLoopStream(mp3Reader);
                else
                    inputStream = new WaveChannel32(mp3Reader);
            }
            else if (filename.EndsWith(".wav"))
            {
                WaveStream waveReader = new WaveFileReader(filename);
                if (waveReader.WaveFormat.Encoding != WaveFormatEncoding.Pcm)
                {
                    waveReader = WaveFormatConversionStream.CreatePcmStream(waveReader);
                    waveReader = new BlockAlignReductionStream(waveReader);
                }

                if (waveReader.WaveFormat.BitsPerSample != 16)
                {
                    var format = new WaveFormat(waveReader.WaveFormat.SampleRate, 16, waveReader.WaveFormat.Channels);
                    waveReader = new WaveFormatConversionStream(format, waveReader);
                }
                inputStream = new WaveChannel32(waveReader);
            }
            else
                throw new InvalidOperationException("[NAudioAdapter] Only mp3 and wav are supported");

            _volumeSytream = inputStream;

            return _volumeSytream;
        }

        private void CloseWaveOut()
        {
            if (_wavePlayer != null)
                _wavePlayer.Stop();

            if (_mainOutputStream != null)
            {
                _volumeSytream.Close();
                _volumeSytream = null;

                _mainOutputStream.Close();
                _mainOutputStream = null;
            }

            if (_wavePlayer != null)
            {
                _wavePlayer.Dispose();
                _wavePlayer = null;
            }
        }

        private void Play(string path)
        {
            _mainOutputStream = CreateInputStream(path);

            _wavePlayer.Init(_mainOutputStream);
            _wavePlayer.Play();
        }

        void IAudioAdapter.PlayMusic(string path)
        {
            Play(path);
        }

        void IAudioAdapter.StopMusic()
        {
            if (_wavePlayer.PlaybackState != PlaybackState.Stopped)
                _wavePlayer.Stop();
        }

        void IAudioAdapter.PauseMusic()
        {
            if (_wavePlayer.PlaybackState == PlaybackState.Playing)
                _wavePlayer.Pause();
        }

        void IAudioAdapter.ResumeMusic()
        {
            if (_wavePlayer.PlaybackState == PlaybackState.Paused)
                _wavePlayer.Play();
        }

        void IAudioAdapter.PlaySound(string path)
        {
            Play(path);
        }

        void IAudioAdapter.PlaySound(string path, float volume, float pitch, float pan)
        {
            Play(path);
        }

        void IDisposable.Dispose()
        {
            CloseWaveOut();
        }
    }
}
