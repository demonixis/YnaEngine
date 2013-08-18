// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Helpers
{
    /// <summary>
    /// A screen helper who provide some methods to get scaled coordinates and scale
    /// </summary>
    public class ScreenHelper
    {
        /// <summary>
        /// Base reference for width
        /// </summary>
        public static int ScreenWidthReference = 640;

        /// <summary>
        /// Base reference for height
        /// </summary>
        public static int ScreenHeightReference = 480;

        /// <summary>
        /// Get the scaled X coordinate relative to the reference width
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetScaleX(float value)
        {
            return (((float)YnG.Width * value) / (float)ScreenWidthReference);
        }

        /// <summary>
        /// Get the scaled Y coordinate relative to the reference height
        /// </summary>
        /// <param name="value">The default Y coordinate used with the reference height</param>
        /// <returns>A scaled Y coordinate</returns>
        public static float GetScaleY(float value)
        {
            return (((float)YnG.Height * value) / (float)ScreenHeightReference);
        }

        /// <summary>
        /// Get the scale relative to the reference width and height
        /// </summary>
        /// <returns>The scale difference between the current resolution and the reference resolution of the screen</returns>
        public static Vector2 GetScale()
        {
            return new Vector2(
                (float)((float)YnG.Width / (float)ScreenWidthReference),
                (float)((float)YnG.Height / (float)ScreenHeightReference));
        }
    }
}
