using System;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SkySphere : IcoSphereGeometry
    {
        public SkySphere(BaseCamera camera, string textureName, float size)
            : base(textureName, 16, 2, false)
        {
            size = Math.Max(1, size);
            _scale *= -1 * size;
        }
    }
}
