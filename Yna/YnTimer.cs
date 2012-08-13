using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Yna
{
    public class YnTimer : YnBase
    {
        #region Private declarations

        private int _duration;
        private int _repeat;
        private int _counter;
        private long _elapsedTime;

        #endregion

        /// <summary>
        /// Get or Set the Duration of the timer
        /// </summary>
        public int Duration 
        {
            get { return _duration; }
            set { _duration = value; }
        }

        /// <summary>
        /// Get or Set the number of time the timer is repeated
        /// </summary>
        public int Repeat 
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        /// <summary>
        /// Get elapsed time since the start of the timer
        /// </summary>
        public long ElapsedTime
        {
            get { return _elapsedTime; }
        }


        /// <summary>
        /// Get the time beforce the timer is completed
        /// </summary>
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

        /// <summary>
        /// Trigger when the timer start
        /// </summary>
        public event EventHandler<EventArgs> Started = null;

        /// <summary>
        /// Trigger when the timer is terminated and restart
        /// </summary>
        public event EventHandler<EventArgs> ReStarted = null;

        /// <summary>
        /// Trigger when the timer is stopped
        /// </summary>
        public event EventHandler<EventArgs> Stopped = null;

        /// <summary>
        /// Trigger when the timer is finish
        /// </summary>
        public event EventHandler<EventArgs> Completed = null;


        public YnTimer(int duration, int repeat = -1)
        {
            Duration = duration;
            Repeat = repeat;
            _elapsedTime = 0;
            _counter = Repeat;
            Active = false;
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        public void Start()
        {
            Active = true;
            TimerStarted(EventArgs.Empty);
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void Stop()
        {
            Active = false;
            TimerStopped(EventArgs.Empty);
        }

        /// <summary>
        /// Pause the timer
        /// </summary>
        public void Kill()
        {
            Active = false;
        }

        #region Events handlers

        private void TimerStarted(EventArgs e)
        {
            if (Started != null)
                Started(this, e);
        }

        private void TimerRestarted(EventArgs e)
        {
            if (ReStarted != null)
                ReStarted(this, e);
        }

        private void TimerStopped(EventArgs e)
        {
            if (Stopped != null)
                Stopped(this, e);
        }

        private void TimerCompleted(EventArgs e)
        {
            if (Completed != null)
                Completed(this, e);
        }

        #endregion

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
