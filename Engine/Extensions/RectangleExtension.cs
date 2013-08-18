// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework
{
    public static class RectangleExtension
    {
        /// <summary>
        /// Gets the top center.
        /// </summary>
        /// <returns>The top center of the rectangle</returns>
        /// <param name='rectangle'></param>
        public static Vector2 GetTopCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y);
        }

        /// <summary>
        /// Gets the bottom center.
        /// </summary>
        /// <returns>The bottom center.</returns>
        /// <param name='rectangle'></param>
        public static Vector2 GetBottomCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height);
        }

        /// <summary>
        /// Gets the left center.
        /// </summary>
        /// <returns>The left center.</returns>
        /// <param name='rectangle'></param>
        public static Vector2 GetLeftCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X, rectangle.Y + rectangle.Height / 2);
        }

        /// <summary>
        /// Gets the right center.
        /// </summary>
        /// <returns>The right center.</returns>
        /// <param name='rectangle'></param>
        public static Vector2 GetRightCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2);
        }

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns>The position.</returns>
        /// <param name='rectangle'></param>
        public static Vector2 ToVector2(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X, rectangle.Y);
        }
    }
}
