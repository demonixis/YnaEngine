using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display;

namespace Yna.Framework.Helpers
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

        public static Color[] GetTextureData (YnObject sceneObject)
        {
            Color[] textureData = new Color[sceneObject.Texture.Width * sceneObject.Texture.Height];
            sceneObject.Texture.GetData(textureData);
            return textureData;
        }

        /// <summary>
        /// Draw a 2D line with a SpriteBatch
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="lineTexture"></param>
        /// <param name="tickness"></param>
        /// <param name="maskColor"></param>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        public static void DrawLine(SpriteBatch batch, Texture2D lineTexture, float tickness, Color maskColor, Vector2 pointA, Vector2 pointB)
        {
            float angle = (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
            float length = Vector2.Distance(pointA, pointB);

            batch.Draw(lineTexture, pointA, null, maskColor, angle, Vector2.Zero, new Vector2(length, tickness), SpriteEffects.None, 0);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Color color, float tickness, Color maskColor, Vector2 pointA, Vector2 pointB)
        {
            DrawLine(spriteBatch, CreateTexture(color, (int)tickness, (int)tickness), tickness, maskColor, pointA, pointB);
        }
    }
}
