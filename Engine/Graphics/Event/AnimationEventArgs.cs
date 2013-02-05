using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics.Event
{
    public class InterpolationEndEventArgs<T> : EventArgs
    {
        public T InterpolatedValue;
        public float ElapsedTime;

        public InterpolationEndEventArgs()
        {
            ElapsedTime = 0.0f;
            InterpolatedValue = default(T);
        }

        public InterpolationEndEventArgs(float elapsedTime, T value)
        {
            ElapsedTime = elapsedTime;
            InterpolatedValue = value;
        }
    }
}
