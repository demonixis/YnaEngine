using System;
using NAudio;
using NAudio.Wave;

namespace Yna.Audio
{
    public class NAudioLoopStream : WaveStream
    {
        private WaveStream _source;
        private bool _repeat;

        public bool Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        public override WaveFormat WaveFormat
        {
            get { return _source.WaveFormat; }
        }

        public override long Length
        {
            get { return _source.Length; }
        }

        public override long Position
        {
            get { return _source.Position; }
            set { _source.Position = value; }
        }

        public NAudioLoopStream(WaveStream source)
        {
            _source = source;
            _repeat = true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = _source.Read(buffer, offset + totalBytesRead, count - totalBytesRead);

                if (bytesRead == 0)
                {
                    if (_source.Position == 0 || !Repeat)
                    {
                        throw new Exception("[NAudioLoopStream] Error");
                    }

                    _source.Position = 0;
                }
                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }
    }
}
