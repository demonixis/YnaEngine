// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Microsoft.Xna.Framework
{
    public static class BoundingBoxExtension
    {
        /// <summary>
        /// Gets the rectangle of the bounding box.
        /// </summary>
        /// <returns>A rectangle of the bounding box</returns>
        /// <param name='boundingBox'></param>
        public static Rectangle ToRectangle(this BoundingBox boundingBox)
        {
            int x = (int)boundingBox.Min.X;
            int y = (int)boundingBox.Min.Z;
            int width = (int)(boundingBox.Max.X - boundingBox.Min.X);
            int height = (int)(boundingBox.Max.Z - boundingBox.Min.Z);

            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Gets the max size of the bounding box.
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <returns>The maximum size of the bounding box.</returns>
        public static float GetMaxSize(this BoundingBox boundingBox)
        {
            float width = boundingBox.Max.X - boundingBox.Min.X;
            float height = boundingBox.Max.Y - boundingBox.Min.Y;
            float depth = boundingBox.Max.Z - boundingBox.Min.Z;
            return Math.Max(width, Math.Max(height, depth));
        }

        /// <summary>
        /// Gets the center of the bounding box.
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <returns>The center of the bounding box</returns>
        public static Vector3 GetCenter(this BoundingBox boundingBox)
        {
            Vector3 center = new Vector3(
                    boundingBox.Max.X - boundingBox.Min.X,
                    boundingBox.Max.Y - boundingBox.Min.Y,
                    boundingBox.Max.Z - boundingBox.Min.Z);
            return center;
        }

        /// <summary>
        /// Gets the bouding sphere of this bouding box.
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <returns>A bounding sphere.</returns>
        public static BoundingSphere ToBoundingSphere(this BoundingBox boundingBox)
        {
            var boundingSphere = new BoundingSphere(boundingBox.GetCenter(), boundingBox.GetMaxSize());
            return boundingSphere;
        }
    }
}

