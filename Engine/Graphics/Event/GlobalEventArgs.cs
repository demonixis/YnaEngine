using System;

namespace Yna.Engine.Graphics.Event
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        public T Value { get; protected set; }

        public ValueChangedEventArgs()
        {
            Value = default(T);
        }

        public ValueChangedEventArgs(T t)
        {
            Value = t;
        }
    }
}
