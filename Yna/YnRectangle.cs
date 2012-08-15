using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna
{
    public class YnRectangle
    {
        /// <summary>
        /// Get or Set the X position 
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Get or Set the Y position
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Get or Set the width of the rectangle
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Get or Set the height of the rectangle
        /// </summary>
        public int Height { get; set; }

        public YnRectangle()
        {
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        public YnRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        // TODO : Finish this class
        // 
    }
}
