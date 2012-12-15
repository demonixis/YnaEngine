using System;

namespace Yna.Framework.Display.Event
{
    public class ScreenChangedEventArgs : EventArgs
    {
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public bool Fullscreen { get; protected set; }

        public ScreenChangedEventArgs(int width, int height, bool fullscreen)
        {
            Width = width;
            Height = height;
            Fullscreen = fullscreen;
        }
    }
}
