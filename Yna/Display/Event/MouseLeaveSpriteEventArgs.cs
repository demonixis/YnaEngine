using System;
using Yna.Display;

namespace Yna.Display.Event
{
    public class MouseLeaveSpriteEventArgs : EventArgs
    {
        public int LastX { get; protected set; }
        public int LastY { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public MouseLeaveSpriteEventArgs(int lastX, int lastY, int x, int y)
        {
            LastX = lastX;
            LastY = lastY;
            X = x;
            Y = y;
        }
    }
}
