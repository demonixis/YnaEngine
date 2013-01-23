using System;
using Microsoft.Xna.Framework.Input;
using Yna.Framework.Input;

namespace Yna.Framework.Display.Event
{
    public class MouseClickSpriteEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public MouseButton MouseButton { get; protected set; }
        public bool JustClicked;
        public bool DoubleClicked;

        public MouseClickSpriteEventArgs(int x, int y, MouseButton mouseButton, bool justClicked, bool doubleClicked)
        {
            X = x;
            Y = y;
            MouseButton = mouseButton;
            JustClicked = justClicked;
            DoubleClicked = doubleClicked;
        }
    }

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
