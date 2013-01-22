using System;
using Yna.Framework.Display.Component;

namespace Yna.Framework.Display.Event
{
    public class VirtualPadPressedEventArgs : EventArgs
    {
        public ControlDirection Direction;

        public VirtualPadPressedEventArgs()
        {
            Direction = ControlDirection.None;
        }

        public VirtualPadPressedEventArgs(ControlDirection direction)
        {
            Direction = direction;
        }
    }
}
