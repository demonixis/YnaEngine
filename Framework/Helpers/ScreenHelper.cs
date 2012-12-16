using System;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Helpers
{
    /// <summary>
    /// Screen helper
    /// </summary>
    public class ScreenHelper
    {
        public static int ScreenWidthReference = 640;
        public static int ScreenHeightReference = 480;

        /// <summary>
        /// Get the scaled X coordinate relative to the reference width
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetScaleX(float value)
        {
            return (((float)YnG.DeviceWidth * value) / (float)ScreenWidthReference);
        }

        /// <summary>
        /// Get the scaled Y coordinate relative to the reference height
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetScaleY(float value)
        {
            return (((float)YnG.DeviceHeight * value) / (float)ScreenHeightReference);
        }

        /// <summary>
        /// Get the scale relative to the reference width and height
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetScale()
        {
            return new Vector2(
                (float)((float)YnG.DeviceWidth / (float)ScreenWidthReference),
                (float)((float)YnG.DeviceHeight / (float)ScreenHeightReference));
        }
    }
}
