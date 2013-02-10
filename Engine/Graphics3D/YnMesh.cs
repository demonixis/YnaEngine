using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    public class YnMesh : YnBase3D
    {
        protected YnEntity3D _entity3D;
        protected BaseMaterial _material;

        public YnMesh(YnEntity3D entity3D, BaseMaterial material)
        {
            _entity3D = entity3D;
            _material = material;
        }

        public override void Update(GameTime gameTime)
        {
            _entity3D.Update(gameTime);
        }
    }
}
