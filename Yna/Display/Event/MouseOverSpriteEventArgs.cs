using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Display.Event
{
    public class MouseOverSpriteEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public MouseOverSpriteEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
