using System;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Event used when the screen resolution change
    /// </summary>
    public class ScreenChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Screen width.
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Screen height.
        /// </summary>
        public int Height { get; protected set; }
        
        /// <summary>
        /// Is in fullscreen mode.
        /// </summary>
        public bool Fullscreen { get; protected set; }

        public ScreenChangedEventArgs(int width, int height, bool fullscreen)
        {
            Width = width;
            Height = height;
            Fullscreen = fullscreen;
        }
    }
}
