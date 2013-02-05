using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class GraphicsDeviceExtension
    {
        public static string ScreenShotFormat = "ScreenShot_{0:000}.png";

        public static void PrepareScreenShot(this GraphicsDevice device)
        {
            if (device.GraphicsProfile == GraphicsProfile.Reach)
                device.SetRenderTarget(new RenderTarget2D(device, device.PresentationParameters.BackBufferWidth, device.PresentationParameters.BackBufferHeight));
        }

        public static void SaveScreenshot(this GraphicsDevice device, string filePath)
        {
            if (device.GraphicsProfile == GraphicsProfile.HiDef)
                SaveScreenShotHiDef(device, filePath);
            else if (device.GraphicsProfile == GraphicsProfile.Reach)
                SaveScreenShotReach(device, filePath);
        }

        internal static void SaveScreenShotHiDef(this GraphicsDevice device, string filePath)
        {
            byte[] data = new byte[device.PresentationParameters.BackBufferWidth * device.PresentationParameters.BackBufferHeight];

        }

        internal static void SaveScreenShotReach(this GraphicsDevice device, string filePath)
        {
            if (device.GetRenderTargets().Length == 0 || device.GetRenderTargets()[0].RenderTarget == null)
                throw new Exception("Error");

            Texture2D texture = (Texture2D)device.GetRenderTargets()[0].RenderTarget;
            device.SetRenderTarget(null);
            SaveTexture(texture, filePath);
        }

#if !WINDOWS_STOREAPP
        
        private static void SaveTexture(Texture2D texture, string filePath)
        {
            using (System.IO.Stream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
            }
        }

#else

        private static void SaveTexture(Texture2D texture, string filePath)
        {

        }
#endif
    }
}
