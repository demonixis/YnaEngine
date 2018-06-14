using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Materials;


namespace Yna.Samples.Screens
{
    public class CylinderSample : BaseSample
    {
        YnMeshGeometry cylinder;
        YnMeshGeometry cone;

        public CylinderSample(string name)
            : base(name, true)
        {
            CylinderGeometry cylinderGeometry = new CylinderGeometry(new Vector3(0, 10, 0), new Vector3(0, 0, 0), 5, 5, false, 10, 10, Vector3.One);
            CylinderGeometry coneGeometry = new CylinderGeometry(new Vector3(0, 10, 0), new Vector3(0, 0, 0), 0, 5, false, 10, 10, Vector3.One);
            cylinder = new YnMeshGeometry(cylinderGeometry, new BasicMaterial("Textures/metal"));
            Add(cylinder);

            cone = new YnMeshGeometry(coneGeometry, new BasicMaterial("Textures/metal"));
            Add(cone);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            cylinder.Position = new Vector3(terrain.Width / 2.5f, 0, terrain.Depth / 2);
            cone.Position = new Vector3(terrain.Width - terrain.Width / 2.5f, 0, terrain.Depth / 2);
        }
    }
}