// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
namespace Microsoft.Xna.Framework
{
    public static class BoundingSphereExtension
    {
        /// <summary>
        /// Gets the minimum value of the bounding sphere.
        /// </summary>
        /// <param name="boudingSphere"></param>
        /// <returns>Return the minimum value of the bounding sphere.</returns>
        public static Vector3 GetMin(this BoundingSphere boudingSphere)
        {
            return new Vector3(
                    boudingSphere.Center.X - boudingSphere.Radius,
                    boudingSphere.Center.Y - boudingSphere.Radius,
                    boudingSphere.Center.Z - boudingSphere.Radius);;
        }

        /// <summary>
        /// Gets the maximum value of the bounding sphere.
        /// </summary>
        /// <param name="boudingSphere"></param>
        /// <returns>Return the maximum value of the bounding sphere.</returns>
        public static Vector3 GetMax(this BoundingSphere boudingSphere)
        {
            return new Vector3(
                    boudingSphere.Center.X + boudingSphere.Radius,
                    boudingSphere.Center.Y + boudingSphere.Radius,
                    boudingSphere.Center.Z + boudingSphere.Radius);;
        }

        /// <summary>
        /// Gets the bounding box of this bounding sphere.
        /// </summary>
        /// <param name="sphere"></param>
        /// <returns>Return a bounding box.</returns>
        public static BoundingBox ToBoundingBox(this BoundingSphere sphere)
        {
            return new BoundingBox(sphere.GetMin(), sphere.GetMax());;
        }
    }
}
