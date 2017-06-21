using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Material;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Samples.Screens
{
    public class MaterialSample : BaseSample
    {
        YnGroup3D groupCube;

        public MaterialSample(string name)
            : base(name, true)
        {
            groupCube = new YnGroup3D(null);
            Add(groupCube);

            var lavaMaterial = new EnvironmentMapMaterial("Textures/pattern02_diffuse", "Textures/lava2_diff");
            var cubeLava = new YnMeshGeometry(new CubeGeometry(2), lavaMaterial);
            cubeLava.Position = new Vector3(25, 2, 50);
            groupCube.Add(cubeLava);

            var waterMaterial = new EnvironmentMapMaterial("Textures/pattern02_diffuse", "Textures/water1a_diff");
            var cubeWater = new YnMeshGeometry(new CubeGeometry(2), waterMaterial);
            cubeWater.Position = new Vector3(35, 2, 50);
            groupCube.Add(cubeWater);
#if !DIRECTX
            var dualMaterial = new DualTextureMaterial("Textures/metal", "Textures/pyramid");
            var cubeDual = new YnMeshGeometry(new CubeGeometry(2), dualMaterial);
            cubeDual.Position = new Vector3(45, 2, 50);
            groupCube.Add(cubeDual);
#endif
            var basicMaterial = new BasicMaterial("Textures/metal");
            var cubeBasic = new YnMeshGeometry(new CubeGeometry(2), basicMaterial);
            cubeBasic.Position = new Vector3(55, 2, 50);
            groupCube.Add(cubeBasic);

            // Setup lighting
            Scene.SceneLight.DirectionalLights[0].DiffuseColor = Color.WhiteSmoke.ToVector3();
            Scene.SceneLight.DirectionalLights[0].DiffuseIntensity = 2.5f;
            Scene.SceneLight.DirectionalLights[0].Direction = new Vector3(1, 0, 0);
            Scene.SceneLight.DirectionalLights[0].SpecularColor = Color.Gray.ToVector3();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            for (int i = 0, l = groupCube.Count; i < l; i++)
                groupCube[i].RotateY(0.01f * (i + 1) * gameTime.ElapsedGameTime.Milliseconds);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}