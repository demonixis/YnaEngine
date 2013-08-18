// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;

namespace Microsoft.Xna.Framework
{
    public static class BoundingFrustrumExtension
    {
        /// <summary>
        /// Make a bounding frustrum of the scene.
        /// </summary>
        /// <param name="frustum"></param>
        /// <param name="camera">Camera used on the scene.</param>
        /// <returns>The bounding frustrum.</returns>
        public static BoundingFrustum BuildBoundingFrustum(this BoundingFrustum frustum, BaseCamera camera)
        {
            Matrix view = Matrix.CreateLookAt(camera.Position, camera.Target, camera.VectorUp);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(camera.FieldOfView, camera.AspectRatio, camera.Near, camera.Far);
            Quaternion rotation = Quaternion.CreateFromRotationMatrix(Matrix.Invert(view));
            Matrix matrix = Matrix.CreateFromQuaternion(rotation);
            matrix.Translation = camera.Position;
            view = Matrix.Invert(matrix);
            return new BoundingFrustum(Matrix.Multiply(view, projection));
        }
    }
}
