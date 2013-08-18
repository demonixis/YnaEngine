// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class ViewportExtension
    {
        /// <summary>
        /// Gets a rectangle from the current viewport.
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns>Return a rectangle from the current viewport.</returns>
        public static Rectangle ToRectangle(this Viewport viewport)
        {
            return new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
        }

        /// <summary>
        /// Gets the center of the current viewport.
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns>Return the center of the current viewport.</returns>
        public static Vector2 GetCenter(this Viewport viewport)
        {
            return new Vector2(viewport.Width / 2, viewport.Height / 2);
        }
    }
}
