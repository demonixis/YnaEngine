using System;
using Microsoft.Xna.Framework;

namespace Yna.Helpers
{
    public class ScreenHelper
    {
        public static int ScreenWidthReference = 640;
        public static int ScreenHeightReference = 480;

        public static float GetScaleX(float value)
        {
            return (((float)YnG.Width * value) / (float)ScreenWidthReference);
        }

        public static float GetScaleY(float value)
        {
            return (((float)YnG.Height * value) / (float)ScreenHeightReference);
        }

        public static Vector2 GetScale()
        {
            return new Vector2(
                (float)((float)YnG.Width / (float)ScreenWidthReference),
                (float)((float)YnG.Height / (float)ScreenHeightReference));
        }
    }
}
