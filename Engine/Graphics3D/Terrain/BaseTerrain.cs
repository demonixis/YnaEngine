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
        public new BaseCamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;
                if (_mesh != null)
                    _mesh.Camera = value;
            }
        }

        public new float Width
        {
            get { return _mesh.Width; }
        }

        public new float Height
        {
            get { return _mesh.Height; }
        }

        public new float Depth
        {
            get { return _mesh.Depth; }
        }

        protected YnGeometryMesh<VertexPositionNormalTexture> _mesh;
        protected BaseTerrainGeometry _geometry;

        public override void LoadContent()
        {
            _geometry.GenerateGeometry();
            _material.LoadContent();
            _mesh = new YnGeometryMesh<VertexPositionNormalTexture>(_geometry, _material);
            _mesh.Camera = Camera;
            UpdateBoundingVolumes();
            _initialized = true;
        }

        public override void UpdateMatrix()
        {
            _mesh.UpdateMatrix();
        }

        public override void UpdateBoundingVolumes()
        {
            _mesh.UpdateBoundingVolumes();
        }

        public override void Draw(GraphicsDevice device)
        {
            _mesh.PreDraw();
            _mesh.Draw(device);
        }
    }
}
