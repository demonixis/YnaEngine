// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// Graphics helper
    /// </summary>
    public class YnGraphics
    {
        /// <summary>
        /// Create an array of color.
        /// </summary>
        /// <param name="desiredColor">Desired color</param>
        /// <param name="width">Desired width</param>
        /// <param name="height">Desired height</param>
        /// <returns>An array of color.</returns>
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
            Texture2D texture2D = new Texture2D(YnG.GraphicsDevice, width, height);
            texture2D.SetData(CreateColor(color, width, height));

            return texture2D;
        }

        /// <summary>
        /// Gets colors of a texture.
        /// </summary>
        /// <param name="texture">A texture</param>
        /// <returns>Array of Color</returns>
        public static Color[] GetTextureColors(Texture2D texture)
        {
            Color[] textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            return textureData;
        }

        /// <summary>
        /// Create a random texture.
        /// </summary>
        /// <param name="resolution">Desired size (the texture will be squared).</param>
        /// <returns>A Texture2D object with the specified size.</returns>
        public static Texture2D CreateRandomTexture(int resolution)
        {
            return CreateRandomTexture(resolution, resolution);
        }

        /// <summary>
        /// Create a random texture.
        /// </summary>
        /// <param name="width">Desired width.</param>
        /// <param name="height">Desired height.</param>
        /// <returns>A Texture2D object with the specified size.</returns>
        public static Texture2D CreateRandomTexture(int width, int height)
        {
            Random rand = new Random();

            Color[] noisyColors = new Color[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                    noisyColors[x + y * width] = new Color(new Vector3((float)rand.Next(1000) / 1000.0f, 0, 0));
            }

            Texture2D noiseImage = new Texture2D(YnG.GraphicsDevice, width, height, false, SurfaceFormat.Color);

            noiseImage.SetData(noisyColors);

            return noiseImage;
        }

        /// <summary>
        /// Draw a line on screen.
        /// </summary>
        /// <param name="batch">SpriteBatch object</param>
        /// <param name="lineTexture">Texture to use</param>
        /// <param name="tickness">Thickness of the line</param>
        /// <param name="maskColor">Mask color</param>
        /// <param name="pointA">Start point</param>
        /// <param name="pointB">End point</param>
        public static void DrawLine(SpriteBatch batch, Texture2D lineTexture, float tickness, Color maskColor, Vector2 pointA, Vector2 pointB)
        {
            float angle = (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
            float length = Vector2.Distance(pointA, pointB);

            batch.Draw(lineTexture, pointA, null, maskColor, angle, Vector2.Zero, new Vector2(length, tickness), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw a line on screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch object</param>
        /// <param name="color">Fill color</param>
        /// <param name="tickness">Thickness of the line</param>
        /// <param name="maskColor">Mask color</param>
        /// <param name="pointA">Start point</param>
        /// <param name="pointB">End point</param>
        public static void DrawLine(SpriteBatch spriteBatch, Color color, float tickness, Color maskColor, Vector2 pointA, Vector2 pointB)
        {
            DrawLine(spriteBatch, CreateTexture(color, (int)tickness, (int)tickness), tickness, maskColor, pointA, pointB);
        }

        /// <summary>
        /// Draw a rectangle on screen.
        /// </summary>
        /// <param name="batch">SpriteBatch object</param>
        /// <param name="texture">Texture to use</param>
        /// <param name="rectangle">Value of the rectangle to draw</param>
        /// <param name="color">Fill color</param>
        /// <param name="borderSize">Border size</param>
        public static void DrawRectangle(SpriteBatch batch, Texture2D texture, Rectangle rectangle, Color color, int borderSize)
        {
            batch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, borderSize), color);
            batch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, borderSize), color);
            batch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, borderSize, rectangle.Height), color);
            batch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top, borderSize, rectangle.Height + 1), color);
        }

        /// <summary>
        /// Draw a rectangle on screen
        /// </summary>
        /// <param name="batch">SpriteBatch object</param>
        /// <param name="rectangle">Value of the rectangle to draw</param>
        /// <param name="color">Fill color</param>
        public static void DrawRectangle(SpriteBatch batch, Rectangle rectangle, Color color)
        {
            Texture2D texture = CreateTexture(Color.White, 1, 1);
            DrawRectangle(batch, texture, rectangle, color, 1);
        }
    }
}
