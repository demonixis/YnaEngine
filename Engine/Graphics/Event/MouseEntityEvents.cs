// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Event class used for entity mouse event.
    /// </summary>
    public class MouseEntityEventArgs : EventArgs
    {
        /// <summary>
        /// X position on screen.
        /// </summary>
        public int X { get; protected set; }

        /// <summary>
        /// Y position on screen.
        /// </summary>
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

    /// <summary>
    /// Event class used when a mouse clic is done on an entity.
    /// </summary>
    public class MouseClickEntityEventArgs : MouseEntityEventArgs
    {
        /// <summary>
        /// The mouse button who clicked.
        /// </summary>
        public MouseButton MouseButton { get; protected set; }

        public bool JustClicked;
        public bool DoubleClicked;

        public MouseClickEntityEventArgs(int x, int y, MouseButton mouseButton, bool justClicked, bool doubleClicked)
            : base(x, y)
        {
            MouseButton = mouseButton;
            JustClicked = justClicked;
            DoubleClicked = doubleClicked;
        }
    }

    /// <summary>
    /// Event class used when the mouse position leave an entity.
    /// </summary>
    public class MouseLeaveEntityEventArgs : MouseEntityEventArgs
    {
        public int LastX { get; protected set; }
        public int LastY { get; protected set; }

        public MouseLeaveEntityEventArgs(int lastX, int lastY, int x, int y)
            : base (x, y)
        {
            LastX = lastX;
            LastY = lastY;
        }
    }

    /// <summary>
    /// Event class used when the mouse is over an entity.
    /// </summary>
    public class MouseOverEntityEventArgs : MouseEntityEventArgs
    {
        public MouseOverEntityEventArgs(int x, int y)
            : base(x, y)
        {

        }
    }

    /// <summary>
    /// Event class used when the mouse button release on an entity.
    /// </summary>
    public class MouseReleaseEntityEventArgs : MouseEntityEventArgs
    {
        public MouseReleaseEntityEventArgs(int x, int y)
            : base(x, y)
        {

        }
    }
}
