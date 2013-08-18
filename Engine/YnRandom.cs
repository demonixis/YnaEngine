// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    /// <summary>
    /// A class for generating random values
    /// </summary>
    public class YnRandom
    {
        private static Random _random = new Random();

        /// <summary>
        /// Gets a random float value between min and max.
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns>A random float value.</returns>
        public static float GetFloat(float min, float max)
        {
            return (float)(_random.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Gets a random float value between float.MinValue and float.MaxValue.
        /// </summary>
        /// <returns>A random float value.</returns>
        public static float GetFloat()
        {
            return GetFloat(float.MinValue, float.MaxValue);
        }

        /// <summary>
        /// Gets a random integer value between min and max.
        /// </summary>
        /// <param name="min">Min value</param>
        /// <param name="max">Max value</param>
        /// <returns>An random integer value.</returns>
        public static int GetInteger(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// Gets a random integer value between int.MinValue and int.MaxValue.
        /// </summary>
        /// <returns>A random integer value.</returns>
        public static int GetInteger()
        {
            return GetInteger(int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// Gets a random Point.
        /// </summary>
        /// <param name="minX">Min X value.</param>
        /// <param name="minY">Min Y value.</param>
        /// <param name="maxX">Max X value.</param>
        /// <param name="maxY">Max Y value.</param>
        /// <returns>A random Point.</returns>
        public static Point GetPoint(int minX, int minY, int maxX, int maxY)
        {
            return new Point(GetInteger(minX, maxX), GetInteger(minY, maxY));
        }

        /// <summary>
        /// Gets a random point.
        /// </summary>
        /// <returns>A random point.</returns>
        public static Point GetPoint()
        {
            return GetPoint(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue);
        }

        /// <summary>
        /// Gets a random Vector2.
        /// </summary>
        /// <param name="minX">Min X value.</param>
        /// <param name="minY">Min Y value.</param>
        /// <param name="maxX">Max X value.</param>
        /// <param name="maxY">Max Y value.</param>
        /// <returns>A random Vector2.</returns>
        public static Vector2 GetVector2(float minX, float minY, float maxX, float maxY)
        {
            return new Vector2(
                GetFloat(minX, maxX),
                GetFloat(minY, maxY));
        }

        /// <summary>
        /// Gets a random Vector2.
        /// </summary>
        /// <returns>A random Vector2 value.</returns>
        public static Vector2 GetVector2()
        {
            return GetVector2(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue);
        }

        /// <summary>
        /// Gets a random Vector3.
        /// </summary>
        /// <param name="minX">Min X value.</param>
        /// <param name="minY">Min Y value.</param>
        /// <param name="minZ">Min Z value.</param>
        /// <param name="maxX">Max X value.</param>
        /// <param name="maxY">Max Y value.</param>
        /// <param name="maxZ">Max Z value.</param>
        /// <returns>A random Vector3.</returns>
        /// <returns></returns>
        public static Vector3 GetVector3(float minX, float minY, float minZ, float maxX, float maxY, float maxZ)
        {
            return new Vector3(GetVector2(minX, minY, maxX, maxY), GetFloat(minZ, maxZ));
        }

        /// <summary>
        /// Gets a random Vector3.
        /// </summary>
        /// <returns>A random Vector3.</returns>
        public static Vector3 GetVector3()
        {
            return GetVector3(float.MinValue, float.MinValue, float.MinValue, float.MaxValue, float.MaxValue, float.MaxValue);
        }

        /// <summary>
        /// Gets a random Vector4.
        /// </summary>
        /// <param name="minX">Min X value.</param>
        /// <param name="minY">Min Y value.</param>
        /// <param name="minZ">Min Z value.</param>
        /// <param name="minW">Min W value.</param>
        /// <param name="maxX">Max X value.</param>
        /// <param name="maxY">Max Y value.</param>
        /// <param name="maxZ">Max Z value.</param>
        /// <param name="maxW">Max W value.</param>
        /// <returns>A random Vector4.</returns>
        public static Vector4 GetVector4(float minX, float minY, float minZ, float minW, float maxX, float maxY, float maxZ, float maxW)
        {
            return new Vector4(GetVector3(minX, minY, minZ, maxX, maxY, maxZ), GetFloat(minW, maxW));
        }

        /// <summary>
        /// Gets a random Vector4.
        /// </summary>
        /// <returns>A random Vector4.</returns>
        public static Vector4 GetVector4()
        {
            return GetVector4(float.MinValue, float.MinValue, float.MinValue, float.MinValue, float.MaxValue, float.MaxValue, float.MaxValue, float.MaxValue);   
        }

        /// <summary>
        /// Gets a random color.
        /// </summary>
        /// <param name="red">Red amount</param>
        /// <param name="green">Green amount</param>
        /// <param name="blue">Blue amount</param>
        /// <param name="alpha">Alpha amount</param>
        /// <returns>A random color.</returns>
        public static Color GetColor(int? red, int? green, int? blue, float? alpha)
        {
            int r = (red != null ? GetInteger(0, (int)red.Value) : GetInteger(0, 255));
            int g = (green != null ? GetInteger(0, (int)green.Value) : GetInteger(0, 255));
            int b = (blue != null ? GetInteger(0, (int)blue.Value) : GetInteger(0, 255));
            int a = (alpha != null ? GetInteger(0, (int)alpha.Value) : GetInteger(0, 255));

            return new Color(r, g, b, a); 
        }

        /// <summary>
        /// Gets a random color.
        /// </summary>
        /// <param name="red">Red amount</param>
        /// <param name="green">Green amount</param>
        /// <param name="blue">Blue amount</param>
        /// <param name="alpha">Alpha amount</param>
        /// <returns>A random color.</returns>
        public static Color GetColor(float? red, float? green, float? blue, float? alpha)
        {
            float r = (red != null) ? (int)(red * 255) % 255 : GetFloat(0, 1);
            float g = (green != null) ? (int)(green * 255) % 255 : GetFloat(0, 1);
            float b = (blue != null) ? (int)(blue * 255) % 255 : GetFloat(0, 1);
            float a = (alpha != null) ? (int)(alpha * 255) % 255 : GetFloat(0, 1);
            
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Gets a random color.
        /// </summary>
        /// <param name="alpha">Alpha amount</param>
        /// <returns>A random color.</returns>
        public static Color GetColor(float alpha)
        {
            return new Color(GetFloat(0.0f, 1.0f), GetFloat(0.0f, 1.0f), GetFloat(0.0f, 1.0f), alpha);
        }

        /// <summary>
        /// Gets a random color with an alpha set to 1.0f;
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color GetColor()
        {
            return GetColor(1.0f);
        }

        /// <summary>
        /// Gets a random Rectangle.
        /// </summary>
        /// <returns>a random Rectangle with random position and random size.</returns>
        public static Rectangle GetRectangle()
        {
            int x = GetInteger();
            int y = GetInteger();
            int width = GetInteger(1, int.MaxValue);
            int height = GetInteger(1, int.MaxValue);

            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Gets a random Rectangle.
        /// </summary>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <returns>A random Rectangle with fixed size.</returns>
        public static Rectangle GetRectangle(int width, int height)
        {
            return new Rectangle(GetInteger(), GetInteger(), width, height);
        }
    }
}

