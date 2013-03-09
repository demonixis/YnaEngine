using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class PyramidSample : BaseSample
    {
        PyramidGeometry pyramid;

        public PyramidSample(string name)
            : base(name)
        {
            pyramid = new PyramidGeometry("Textures/pyramid", new Vector3(10), Vector3.Zero);
            pyramid.TextureRepeat = new Vector2(4);
            Add(pyramid);

            CylinderGeometry cylinder = new CylinderGeometry("Textures/Sky", new Vector3(0, 10, 0), new Vector3(0, 0, 0), 5, 5, false, 10, 10, Vector3.One, Vector3.Zero);
            Add(cylinder);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            pyramid.Position = new Vector3(terrain.Width / 2, pyramid.Height, terrain.Depth / 2);
        }
    }
}