using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class CylinderSample : BaseSample
    {
        CylinderGeometry cylinder;
        CylinderGeometry cone;

        public CylinderSample(string name)
            : base(name)
        {
            cylinder = new CylinderGeometry("Textures/metal", new Vector3(0, 10, 0), new Vector3(0, 0, 0), 5, 5, false, 10, 10, Vector3.One, Vector3.Zero);
            Add(cylinder);

            cone = new CylinderGeometry("Textures/metal", new Vector3(0, 10, 0), new Vector3(0, 0, 0), 0, 5, false, 10, 10, Vector3.One, Vector3.Zero);
            Add(cone);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            cylinder.Position = new Vector3(terrain.Width / 2.5f, cylinder.Height / 2, terrain.Depth / 2);
            cone.Position = new Vector3(terrain.Width - terrain.Width / 2.5f, cone.Height / 2, terrain.Depth / 2);
        }
    }
}