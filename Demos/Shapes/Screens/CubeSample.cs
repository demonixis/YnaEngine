using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Materials;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class CubeSample : BaseSample
    {
        YnGroup3D groupCube;

        public CubeSample(string name)
            : base(name, true)
        {
            groupCube = new YnGroup3D(null);

            YnMeshGeometry cube = null;
            Vector3 cubePosition = new Vector3(10, 1, 35);

            for (int i = 0; i < 12; i++)
            {
                cubePosition.X +=  5;             
                cube = new YnMeshGeometry(new CubeGeometry(Vector3.One), new BasicMaterial("Textures/pattern02_diffuse"));
                cube.Position = cubePosition;
                groupCube.Add(cube);
            }

            Add(groupCube);

            // Setup a new material for the terrain
            BasicMaterial terrainMaterial = new BasicMaterial("Textures/pattern55_diffuse");
            terrainMaterial.FogColor = Color.White.ToVector3();
            terrainMaterial.FogStart = 15.0f;
            terrainMaterial.FogEnd = 65.0f;
            terrainMaterial.EnableFog = true;
            terrain.Material = terrainMaterial;

            // Setup lighting
            SceneLight.DirectionalLights[0].DiffuseColor = Color.WhiteSmoke.ToVector3();
            SceneLight.DirectionalLights[0].DiffuseIntensity = 2.5f;
            SceneLight.DirectionalLights[0].Direction = new Vector3(1, 0, 0);
            SceneLight.DirectionalLights[0].SpecularColor = Color.Gray.ToVector3();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0, l = groupCube.Count; i < l; i++)
                groupCube[i].RotateY(0.01f * (i + 1) * gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}