using System;

namespace Microsoft.Xna.Framework
{
    public static class BoundingBoxExtension
    {
        /// <summary>
        /// Get the rectangle of the bounding box
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
    }
}

