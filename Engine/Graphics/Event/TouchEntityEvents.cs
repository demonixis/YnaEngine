// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Base class for a touch event.
    /// </summary>
    public class TouchEntityEventArgs : EventArgs
    {
        /// <summary>
        /// X coordinate where finger press/release.
        /// </summary>
        public int X { get; protected set; }

        /// <summary>
        /// Y coordinate where finger press/release.
        /// </summary>
        public int Y { get; protected set; }

        /// <summary>
        /// The finger id.
        /// </summary>
        public int FingerId { get; protected set; }

        /// <summary>
        /// Create an empty event
        /// </summary>
        public TouchEntityEventArgs()
            : this(0, 0, -1)
        {

        }

        /// <summary>
        /// Create a touch event with a position and a finger identifier.
        /// </summary>
        public TouchEntityEventArgs(int x, int y, int fingerId)
        {
            X = x;
            Y = y;
            FingerId = fingerId;
        }
    }

    /// <summary>
    /// Event used when a finger is touching an entity
    /// </summary>
    public class TouchActionEntityEventArgs : TouchEntityEventArgs
    {
        /// <summary>
        /// Determine if the entity is touched
        /// </summary>
        public bool Pressed { get; protected set; }

        /// <summary>
        /// Determine if a finger has moved over an entity
        /// </summary>
        public bool Moved { get; protected set; }

        /// <summary>
        /// Determine if a finger has released 
        /// </summary>
        public bool Released { get; protected set; }

        /// <summary>
        /// Gets the pressure level.
        /// </summary>
        public float Pressure { get; protected set; }

        public TouchActionEntityEventArgs()
            : this(0, 0, -1, false, false, false, 0.0f)
        {

        }

        public TouchActionEntityEventArgs(int x, int y, int fingerId, bool pressed, bool moved, bool released, float pressure)
            : base(x, y, fingerId)
        {
            Pressed = pressed;
            Moved = moved;
            Released = released;
            Pressure = pressure;
        }
    }

    /// <summary>
    /// Event used when a finger leave an entity
    /// </summary>
    public class TouchLeaveEntityEventArgs : TouchEntityEventArgs
    {
        /// <summary>
        /// Last X coordinate on screen
        /// </summary>
        public int LastX { get; protected set; }

        /// <summary>
        /// Last Y coordinate on screen
        /// </summary>
        public int LastY { get; protected set; }


        public TouchLeaveEntityEventArgs()
            : this(0, 0, -1, 0, 0)
        {

        }

        public TouchLeaveEntityEventArgs(int lastX, int lastY, int fingerId, int x, int y)
            : base(x, y, fingerId)
        {
            LastX = lastX;
            LastY = lastY;
        }
    }
}
