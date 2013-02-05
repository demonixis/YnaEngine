using System;
using Yna.Engine.Graphics.Component;

namespace Yna.Engine.Graphics.Event
{
    public class VirtualPadPressedEventArgs : EventArgs
    {
        public PadButtons Direction;

        public VirtualPadPressedEventArgs()
        {
            Direction = PadButtons.None;
        }

        public VirtualPadPressedEventArgs(PadButtons direction)
        {
            Direction = direction;
        }
    }
}
