// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Yna.Engine.Graphics.Component;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Event for virtul pad controller who're used to notify the direction.
    /// </summary>
    public class VirtualPadPressedEventArgs : EventArgs
    {
        public PadButtons Direction { get; set; }

        /// <summary>
        /// Create an empty event.
        /// </summary>
        public VirtualPadPressedEventArgs()
        {
            Direction = PadButtons.None;
        }

        /// <summary>
        /// Create an event with a direction.
        /// </summary>
        /// <param name="direction"></param>
        public VirtualPadPressedEventArgs(PadButtons direction)
        {
            Direction = direction;
        }
    }
}
