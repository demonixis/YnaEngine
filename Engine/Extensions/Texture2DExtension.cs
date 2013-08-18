// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class Texture2DExtension
    {
        /// <summary>
        /// Gets the source rectangle of the texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>A rectangle that begin at 0, 0</returns>
        public static Rectangle GetSourceRectangle(this Texture2D texture)
        {
            return new Rectangle(0, 0, texture.Width, texture.Height);
        }

        /// <summary>
        /// Gets the size of the texture.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>Return a point that contains the size of the texture</returns>
        public static Point GetSize(this Texture2D texture)
        {
            return new Point(texture.Width, texture.Height);
        }
    }
}
