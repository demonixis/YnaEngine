using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    public class YnGeometryMesh<T> : YnMesh where T : struct, IVertexType
    {
        private BaseGeometry<T> _geometry;

        public YnGeometryMesh(BaseGeometry<T> geometry)
            : this(geometry, "")
        {

        }

        public YnGeometryMesh(BaseGeometry<T> geometry, string textureName)
            : this(geometry, new BasicMaterial(textureName))
        {

        }

        public YnGeometryMesh(BaseGeometry<T> geometry, BaseMaterial material)
            : base()
        {
            _geometry = geometry;
            _material = material;
        }

        public override void LoadContent()
        {
            _geometry.GenerateGeometry();
            _material.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


        }

        public override void Draw(GraphicsDevice device)
        {
            _geometry.Draw(device, _material);
        }
    }
}
