using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Samples.Screens
{
    public class SphereSample : BaseSample
    {
        YnMeshGeometry<VertexPositionNormalTexture> sphere;

        public SphereSample(string name)
            : base(name)
        {
            SphereGeometry geometry = new SphereGeometry(10);
            geometry.TessellationLevel = 20;

            BasicMaterial material = new BasicMaterial("Textures/metal");

            sphere = new YnMeshGeometry<VertexPositionNormalTexture>(geometry, material);
            Add(sphere);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            sphere.Position = new Vector3(terrain.Width / 2, sphere.Height, terrain.Depth / 2);
        }
    }
}