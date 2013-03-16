using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;


namespace Yna.Samples.Screens
{
    public class CylinderSample : BaseSample
    {
        YnMeshGeometry<VertexPositionNormalTexture> cylinder;
        YnMeshGeometry<VertexPositionNormalTexture> cone;

        public CylinderSample(string name)
            : base(name, true)
        {
            CylinderGeometry cylinderGeometry = new CylinderGeometry(new Vector3(0, 10, 0), new Vector3(0, 0, 0), 5, 5, false, 10, 10, Vector3.One);
            CylinderGeometry coneGeometry = new CylinderGeometry(new Vector3(0, 10, 0), new Vector3(0, 0, 0), 0, 5, false, 10, 10, Vector3.One);
            cylinder = new YnMeshGeometry<VertexPositionNormalTexture>(cylinderGeometry, new BasicMaterial("Textures/metal"));
            Add(cylinder);

            cone = new YnMeshGeometry<VertexPositionNormalTexture>(coneGeometry, new BasicMaterial("Textures/metal"));
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