using System;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SkySphere : YnGeometryMesh<VertexPositionNormalTexture>
    {
        public SkySphere(BaseCamera camera, string textureName, float size)
            : base(new IcoSphereGeometry(16, 2, true), new BasicMaterial(textureName))
        {

        }
    }
}
