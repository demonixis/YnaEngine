using System;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SkySphere : YnMeshGeometry
    {
        public SkySphere(string textureName, float size)
            : base(new IcoSphereGeometry(5, 3, true), new BasicMaterial(textureName))
        {
            _scale *= -1 * size;
        }
    }
}
