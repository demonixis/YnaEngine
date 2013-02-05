using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input.Touch;

namespace Yna.Engine.Graphics.Event
{
    public class TouchActionSpriteEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public bool Pressed { get; protected set; }
        public bool Moved { get; protected set; }
        public bool Released { get; protected set; }
        public float Pressure { get; protected set; }

        public TouchActionSpriteEventArgs(int x, int y, bool pressed, bool moved, bool released, float pressure)
        {
            X = x;
            Y = y;
            Pressed = pressed;
            Moved = moved;
            Released = released;
            Pressure = pressure;
        }
    }

    public class TouchLeaveSpriteEventArgs : EventArgs
    {
        public int LastX { get; protected set; }
        public int LastY { get; protected set; }
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int FingerId { get; protected set; }

        public TouchLeaveSpriteEventArgs(int lastX, int lastY, int x, int y, int fingerId)
        {
            LastX = lastX;
            LastY = lastY;
            X = x;
            Y = y;
            FingerId = fingerId;
        }
    }

    public class TouchOverSpriteEventArgs : EventArgs
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int FingerId { get; protected set; }

        public TouchOverSpriteEventArgs(int x, int y, int fingerId)
        {
            X = x;
            Y = y;
            FingerId = fingerId;
        }
    }
}
