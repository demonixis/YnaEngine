using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometries;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Samples.Screens
{
    public class SphereSample : BaseSample
    {
        YnMeshGeometry sphere;

        public SphereSample(string name)
            : base(name)
        {
            var geometry = new SphereGeometry(10, 20);
            var material = new BasicMaterial("Textures/metal");

            sphere = new YnMeshGeometry(geometry, material);
            Add(sphere);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            sphere.Position = new Vector3(terrain.Width / 2, sphere.Height / 2, terrain.Depth / 2);
        }
    }
}