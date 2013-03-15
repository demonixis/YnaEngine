using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain
{
    /// <summary>
    /// Abstract class that represent a basic Terrain
    /// </summary>
    public abstract class BaseTerrain : YnEntity3D
    {
        protected YnGeometryMesh<VertexPositionNormalTexture> _mesh;
        protected BaseTerrainGeometry _geometry;

        public override void LoadContent()
        {
            _geometry.GenerateGeometry();
            _material.LoadContent();
        }

        public override void UpdateMatrix()
        {
            throw new NotImplementedException();
        }

        public override void UpdateBoundingVolumes()
        {
            throw new NotImplementedException();
        }

        public override void Draw(GraphicsDevice device)
        {
            _mesh.Draw(device);
        }
    }
}
