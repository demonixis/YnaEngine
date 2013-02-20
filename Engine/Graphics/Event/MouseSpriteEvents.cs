using System;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Base event used for entity mouse event.
    /// </summary>
    public class MouseEntityEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public MouseEntityEventArgs()
        {
            X = 0;
            Y = 0;
        }

        public MouseEntityEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class MouseClickEntityEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public MouseButton MouseButton { get; protected set; }
        public bool JustClicked;
        public bool DoubleClicked;

        public MouseClickEntityEventArgs(int x, int y, MouseButton mouseButton, bool justClicked, bool doubleClicked)
        {
            X = x;
            Y = y;
            MouseButton = mouseButton;
            JustClicked = justClicked;
            DoubleClicked = doubleClicked;
        }
    }

    public class MouseLeaveEntityEventArgs : EventArgs
    {
        public int LastX { get; protected set; }
        public int LastY { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public MouseLeaveEntityEventArgs(int lastX, int lastY, int x, int y)
        {
            LastX = lastX;
            LastY = lastY;
            X = x;
            Y = y;
        }
    }

    public class MouseOverEntityEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public MouseOverEntityEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class MouseReleaseEntityEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public MouseReleaseEntityEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
