// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
namespace Microsoft.Xna.Framework
{
    public static class PointExtension
    {
        /// <summary>
        /// Gets a point from a Vector2
        /// </summary>
        /// <param name="point"></param>
        /// <returns>Return a point.</returns>
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}
