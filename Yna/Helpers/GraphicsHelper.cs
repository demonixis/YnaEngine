using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display;

namespace Yna.Helpers
{
    class GraphicsHelper
    {
        public static Color[] CreateColor (Color textureColor, int width, int height)
        {
            Color[] color = new Color[width * height];
            for (int i = 0; i < color.Length; i++)
                color[i] = textureColor;
            return color;
        }

        public static Texture2D CreateTexture (Color color, int width, int height)
        {
            Texture2D texture2D = new Texture2D(YnG.GraphicsDeviceManager.GraphicsDevice, width, height);
            texture2D.SetData(CreateColor(color, width, height));

            return texture2D;
        }

        public static Color[] GetTextureData (Sprite s)
        {
            Color[] textureData = new Color[s.Texture.Width * s.Texture.Height];
            s.Texture.GetData(textureData);
            return textureData;
        }
    }
}
