﻿using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;

namespace Microsoft.Xna.Framework
{
    public static class BoundingFrustrumExtension
    {
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