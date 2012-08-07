using System;
using Microsoft.Xna.Framework.Input;
using Yna.Input;

namespace Yna.Display.Event
{
    public class MouseClickSpriteEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public MouseButton MouseButton { get; protected set; }
        public bool JustClicked;
        public bool DoubleClicked;

        public MouseClickSpriteEventArgs(int x, int y, MouseButton mouseButton, bool justClicked = false, bool doubleClicked = false)
        {
            X = x;
            Y = y;
            MouseButton = mouseButton;
            JustClicked = justClicked;
            DoubleClicked = doubleClicked;
        }
    }
}
