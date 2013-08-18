// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Terrain.Geometry;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SimpleTerrain : BaseTerrain
    {
        public SimpleTerrain(string textureName, int width, int depth)
            : this (textureName, width, depth, 1, 1)
        {

        }

        public SimpleTerrain(string textureName, int width, int depth, int segmentX, int segmentZ)
            : base(width, 0, depth)
        {
            _geometry = new SimpleTerrainGeometry(width, 0, depth, new Vector3(segmentX, 0, segmentZ));
            _material = new BasicMaterial(textureName);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            base.Draw(gameTime, device, camera);
        }
    }
}
