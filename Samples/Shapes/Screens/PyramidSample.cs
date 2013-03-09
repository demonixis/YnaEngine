using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class PyramidSample : BaseSample
    {
        PyramidGeometry pyramid;

        public PyramidSample(string name)
            : base(name, true)
        {
            pyramid = new PyramidGeometry("Textures/pyramid", new Vector3(10), Vector3.Zero);
            pyramid.TextureRepeat = new Vector2(4);
            Add(pyramid);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            pyramid.Position = new Vector3(terrain.Width / 2, pyramid.Height, terrain.Depth / 2);
        }
    }
}