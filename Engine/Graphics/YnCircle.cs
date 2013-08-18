// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
namespace Yna.Engine.Graphics
{
    /// <summary>
    /// Define a circle with coordinate and radius
    /// </summary>
    public class YnCircle
    {
        /// <summary>
        /// Gets or sets the X coordinate of the circle
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate of the circle
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the radius of the circle
        /// </summary>
        public int Radius { get; set; }

        /// <summary>
        /// Create a new circle with default value 0, 0, 0
        /// </summary>
        public YnCircle()
        {
            X = 0;
            Y = 0;
            Radius = 0;
        }

        /// <summary>
        /// Create a new circle
        /// </summary>
        /// <param name="x">Position on X</param>
        /// <param name="y">Position on Y</param>
        /// <param name="radius">Radius</param>
        public YnCircle(int x, int y, int radius)
        {
            X = x;
            Y = y;
            Radius = radius;
        }

        /// <summary>
        /// Check if a point is in the circle
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="circle">A circle</param>
        /// <returns>True if collide then false</returns>
        public static bool Collide(int x, int y, YnCircle circle)
        {
            int r2 = (x - circle.X) * (x - circle.X) + (y - circle.Y) * (y - circle.Y);
            return r2 > (circle.Radius * circle.Radius);
        }

        /// <summary>
        /// Check if two circles are in collision
        /// </summary>
        /// <param name="circleA"></param>
        /// <param name="circleB"></param>
        /// <returns></returns>
        public static bool Collide(YnCircle circleA, YnCircle circleB)
        {
            int r2 = (circleA.X - circleB.X) * (circleA.X - circleB.X) + (circleA.Y - circleB.Y) * (circleA.Y - circleB.Y);
            return r2 > ((circleA.Radius + circleB.Radius) * (circleA.Radius + circleB.Radius));
        }
    }
}
