using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Framework.Display.Event
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
