using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display;

namespace Yna.Framework.Display
{
    public class YnGraphics
    {
        public static Color[] CreateColor(Color desiredColor, int width, int height)
        {
            Color[] color = new Color[width * height];
            for (int i = 0; i < color.Length; i++)
                color[i] = desiredColor;
            return color;
        }

        /// <summary>
        /// Create a texture with a color and a size specified in parameter
        /// </summary>
        /// <param name="color">Color of the texture</param>
        /// <param name="width">Width of the texture</param>
        /// <param name="height">Height of the texture</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(Color color, int width, int height)
        {
            Texture2D texture2D = new Texture2D(YnG.GraphicsDeviceManager.GraphicsDevice, width, height);
            texture2D.SetData(CreateColor(color, width, height));

            return texture2D;
        }

        /// <summary>
        /// Get color for each pixel from a texture
        /// </summary>
        /// <param name="texture">A texture</param>
        /// <returns>Array of color</returns>
        public static Color[] GetTextureData(Texture2D texture)
        {
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
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

        public static void DrawRectangle(SpriteBatch batch, Texture2D texture, Rectangle rectangle, Color color, int borderSize)
        {
            batch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, borderSize), color);
            batch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, borderSize), color);
            batch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, borderSize, rectangle.Height), color);
            batch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top, borderSize, rectangle.Height + 1), color);
        }

        public static void DrawRectangle(SpriteBatch batch, Rectangle rectangle, Color color)
        {
            Texture2D texture = CreateTexture(Color.White, 1, 1);
            DrawRectangle(batch, texture, rectangle, color, 1);
        }
    }
}
