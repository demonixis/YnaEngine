using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class PlaneSample : BaseSample
    {
        PlaneGeometry plane;

        public PlaneSample(string name)
            : base(name)
        {
            plane = new PlaneGeometry("Textures/metal", new Vector3(10), Vector3.Zero);
            Add(plane);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            plane.Position = new Vector3(terrain.Width / 2, 0.5f, terrain.Depth / 2);
        }
    }
}