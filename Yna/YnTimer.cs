using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Yna
{
    public class YnTimer : YnBase
    {
        private static uint _id = 0;
        private long _elapsedTime;
        private int _counter;

        public int Duration { get; set; }
        public int Repeat { get; set; }

        public long ElapsedTime
        {
            get { return _elapsedTime; }
        }

        public long TimeRemaining
        {
            get
            {
                if (_elapsedTime == 0)
                    return 0;
                else
                    return (long)(Duration - ElapsedTime);
            }
        }

        public event EventHandler<EventArgs> Started = null;
        public event EventHandler<EventArgs> ReStarted = null;
        public event EventHandler<EventArgs> Stopped = null;
        public event EventHandler<EventArgs> Completed = null;

        public YnTimer(int duration, int repeat = -1)
        {
            Id = _id++;
            Duration = duration;
            Repeat = repeat;
            _elapsedTime = 0;
            _counter = Repeat;
            Active = false;
        }

        public void Start()
        {
            Active = true;
            TimerStarted(EventArgs.Empty);
        }

        public void Stop()
        {
            Active = false;
            TimerStopped(EventArgs.Empty);
        }

        public void Kill()
        {
            Active = false;
        }

        public void TimerStarted(EventArgs e)
        {
            if (Started != null)
                Started(this, e);
        }

        public void TimerRestarted(EventArgs e)
        {
            if (ReStarted != null)
                ReStarted(this, e);
        }

        public void TimerStopped(EventArgs e)
        {
            if (Stopped != null)
                Stopped(this, e);
        }

        public void TimerCompleted(EventArgs e)
        {
            if (Completed != null)
                Completed(this, e);
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_elapsedTime >= Duration)
                {
                    if (_counter == 0)
                    {
                        Active = false;
                    }
                    else
                    {
                        _counter--;
                        TimerRestarted(EventArgs.Empty);
                    }

                    TimerCompleted(EventArgs.Empty);
                    _elapsedTime = 0;
                }
            }
        }
    }
}
