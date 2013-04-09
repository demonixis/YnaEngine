using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

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
            BasicMaterial terrainMaterial = new BasicMaterial("Textures/pattern55_diffuse");
            terrainMaterial.FogColor = Color.White.ToVector3();
            terrainMaterial.FogStart = 15.0f;
            terrainMaterial.FogEnd = 65.0f;
            terrainMaterial.EnableFog = true;
            terrain.Material = terrainMaterial;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            // Set the camera position at the middle of the terrain
            icosphere.Position = new Vector3(terrain.Width / 2, icosphere.Scale.Y, terrain.Depth / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            icosphere.RotateY(gameTime.ElapsedGameTime.Milliseconds * 0.1f);
        }
    }
}