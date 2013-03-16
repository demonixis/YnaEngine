using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SimpleTerrain : BaseTerrain
    {
        public SimpleTerrain(string textureName, int width, int depth)
            : this (textureName, width, depth, 1, 1)
        {

        }

        public SimpleTerrain(string textureName, int width, int depth, int segmentX, int segmentZ)
        {
            _width = width;
            _height = 0;
            _depth = depth;
            _geometry = new SimpleTerrainGeometry(new Vector3(segmentX, 0, segmentZ));
            _geometry.Dimension = new Vector3(_width, _height, _depth);
            _material = new BasicMaterial(textureName);
        } 
    }
}
