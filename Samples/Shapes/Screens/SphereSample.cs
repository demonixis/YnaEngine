using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class SphereSample : BaseSample
    {
        SphereGeometry sphere;

        public SphereSample(string name)
            : base(name)
        {
            sphere = new SphereGeometry("Textures/metal", 10);
            sphere.TessellationLevel = 20;
            Add(sphere);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            sphere.Position = new Vector3(terrain.Width / 2, sphere.Height, terrain.Depth / 2);
        }
    }
}