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
        {
            X = 0;
            Y = 0;
            FingerId = -1;
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
    public class TouchActionSpriteEventArgs : TouchEntityEventArgs
    {
        public bool Pressed { get; protected set; }
        public bool Moved { get; protected set; }
        public bool Released { get; protected set; }
        public float Pressure { get; protected set; }

        public TouchActionSpriteEventArgs(int x, int y, int fingerId, bool pressed, bool moved, bool released, float pressure)
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
        public int LastX { get; protected set; }
        public int LastY { get; protected set; }

        public TouchLeaveEntityEventArgs(int lastX, int lastY, int x, int y, int fingerId)
            : base(x, y, fingerId)
        {
            LastX = lastX;
            LastY = lastY;
        }
    }
}
