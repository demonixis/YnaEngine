using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometries;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Samples.Screens
{
    public class IcosphereSample : BaseSample
    {
        YnMeshGeometry icosphere;

        public IcosphereSample(string name)
            : base(name, true)
        {
            icosphere = new YnMeshGeometry(new IcoSphereGeometry(32, 4, false), new BasicMaterial("Textures/icosphere_map"));
            icosphere.Scale = new Vector3(3.5f);
            Add(icosphere);

            // Setup a new material for the terrain
            terrain.Material = new BasicMaterial("Textures/pattern55_diffuse");
            SetFog(true, Color.White);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            icosphere.Position = new Vector3(terrain.Width / 2, icosphere.Scale.Y, terrain.Depth / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            icosphere.RotateY(gameTime.ElapsedGameTime.Milliseconds * 0.1f);
        }
    }
}