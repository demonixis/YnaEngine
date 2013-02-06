using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class GraphicsDeviceExtension
    {
        public static string ScreenShotFormat = "ScreenShot_{0:000}.png";

        /// <summary>
        /// Prepares the the graphics device for take a screenshot
        /// </summary>
        /// <param name='device'></param>
        public static void PrepareScreenShot(this GraphicsDevice device)
        {
            if (device.GraphicsProfile == GraphicsProfile.Reach)
                device.SetRenderTarget(new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
        }

        /// <summary>
        /// Save the current screen to a screenshot.
        /// </summary>
        /// <param name="filePath">Path to save the screenshot</param>
        /// <param name='device'></param>
        public static void SaveScreenshot(this GraphicsDevice device, string filePath)
        {
            if (device.GraphicsProfile == GraphicsProfile.HiDef)
                SaveScreenShotHiDef(device, filePath);
            else if (device.GraphicsProfile == GraphicsProfile.Reach)
                SaveScreenShotReach(device, filePath);
        }

        /// <summary>
        /// Save the current screen to a screenshot with hidef profile.
        /// </summary>
        /// <param name="filePath">Path to save the screenshot</param>
        /// <param name='device'></param>
        private static void SaveScreenShotHiDef(this GraphicsDevice device, string filePath)
        {
            byte[] data = new byte[device.PresentationParameters.BackBufferWidth * device.PresentationParameters.BackBufferHeight];

        }

        /// <summary>
        /// Save the current screen to a screenshot with reach profile.
        /// </summary>
        /// <param name="filePath">Path to save the screenshot</param>
        /// <param name='device'></param>
        private static void SaveScreenShotReach(this GraphicsDevice device, string filePath)
        {
            if (device.GetRenderTargets().Length == 0 || device.GetRenderTargets()[0].RenderTarget == null)
                throw new Exception("Error");

            Texture2D texture = (Texture2D)device.GetRenderTargets()[0].RenderTarget;
            device.SetRenderTarget(null);
            SaveTexture(texture, filePath);
        }

#if !WINDOWS_STOREAPP && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_8

        /// <summary>
        /// Save the current screen to a screenshot with hidef profile.
        /// </summary>
        /// <param name="texture">A Texture2D to save</param>
        /// <param name="filePath">Path to save the screenshot</param>
        private static void SaveTexture(Texture2D texture, string filePath)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }
        }

#else
        /// <summary>
        /// Save the current screen to a screenshot with hidef profile.
        /// </summary>
        /// <param name="texture">A Texture2D to save</param>
        /// <param name="filePath">Path to save the screenshot</param>
        private static void SaveTexture(Texture2D texture, string filePath)
        {

        }
#endif
    }
}
