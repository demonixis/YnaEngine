using System;

namespace Microsoft.Xna.Framework
{
    public static class Vector2Extension
    {
        /// <summary>
        /// Gets the angle value between to vectors
        /// </summary>
        /// <returns>Angle between two vectors</returns>
        /// <param name='va'></param>
        /// <param name='vb'></param>
        public static double GetAngle(this Vector2 va, Vector2 vb)
        {
            return Math.Atan2((vb.Y - va.Y) * -1, vb.X - va.X);
        }
    }
}
