using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display;

namespace Yna.Helpers
{
    public class GraphicsHelper
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

        public static Color[] GetTextureData (YnSprite s)
        {
            Color[] textureData = new Color[s.Texture.Width * s.Texture.Height];
            s.Texture.GetData(textureData);
            return textureData;
        }

        public static void DrawLine(SpriteBatch batch, Texture2D lineTexture, float tickness, Color color, Vector2 pointA, Vector2 pointB)
        {
            float angle = (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
            float length = Vector2.Distance(pointA, pointB);

            batch.Draw(lineTexture, pointA, null, color, angle, Vector2.Zero, new Vector2(length, tickness), SpriteEffects.None, 0);
        }
    }
}
