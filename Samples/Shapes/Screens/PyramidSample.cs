using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Samples.Screens
{
    public class PyramidSample : BaseSample
    {
        YnMeshGeometry pyramid;

        public PyramidSample(string name)
            : base(name, true)
        {
            PyramidGeometry geometry = new PyramidGeometry(new Vector3(10));
            geometry.TextureRepeat = new Vector2(4);

            pyramid = new YnMeshGeometry(geometry, new BasicMaterial("Textures/pyramid"));
            Add(pyramid);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            pyramid.Position = new Vector3(terrain.Width / 2, pyramid.Height, terrain.Depth / 2);
        }
    }
}