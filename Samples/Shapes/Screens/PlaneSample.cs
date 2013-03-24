using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Samples.Screens
{
    public class PlaneSample : BaseSample
    {
        YnMeshGeometry plane;

        public PlaneSample(string name)
            : base(name)
        {
            plane = new YnMeshGeometry(new PlaneGeometry(new Vector3(10)), new BasicMaterial("Textures/metal"));
            Add(plane);

            YnBillboard b = new YnBillboard(new Engine.Graphics.YnSprite("Textures/Sky"));
            Add(b);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            plane.Position = new Vector3(terrain.Width / 2, 0.5f, terrain.Depth / 2);
        }
    }
}