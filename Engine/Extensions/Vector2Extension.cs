// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
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

        /// <summary>
        /// Gets a point from a Vector2
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Return a point with casted value from Vector2.</returns>
        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
        
        /// <summary>
        /// Gets a transformed position from a standard plan to isometric plan.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Vector2 ToIso(float x, float y)
	    {
	        Vector2 vec = Vector2.Zero;
	    
	        float vy = (2 * y - x) / 2;
	        float vx = (x + vy);
	    
	        return new Vector2(vx, vy);
	    }
    }
}
