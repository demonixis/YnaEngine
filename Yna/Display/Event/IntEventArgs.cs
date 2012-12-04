using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Display.Event
{
    public class IntEventArgs : EventArgs
    {
        public int Value { get; protected set; }
        public IntEventArgs(int v)
        {
            Value = v;
        }
    }
}
