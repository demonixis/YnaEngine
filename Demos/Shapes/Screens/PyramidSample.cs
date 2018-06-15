using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Samples.Screens
{
    public class PyramidSample : BaseSample
    {
        YnEntity3DGeometry pyramid;

        public PyramidSample(string name)
            : base(name, true)
        {
            PyramidGeometry geometry = new PyramidGeometry(new Vector3(10));
            geometry.TextureRepeat = new Vector2(4);

            string[] textures = new string[6]
            {
                    "Textures/galaxy/galaxy-X",
                    "Textures/galaxy/galaxy+X",
                    "Textures/galaxy/galaxy-Y",
                    "Textures/galaxy/galaxy+Y",
                    "Textures/galaxy/galaxy-Z",
                    "Textures/galaxy/galaxy+Z"
                };

            var material = new EnvironmentMapMaterial("Textures/pyramid", textures);
            material.Amount = 0.95f;
            material.PreferPerPixelLighting = true;
            material.FresnelFactor = 0.2f;

            pyramid = new YnEntity3DGeometry(geometry, material);
            Add(pyramid);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            pyramid.Position = new Vector3(terrain.Width / 2, pyramid.Height / 2, terrain.Depth / 2);
        }
    }
}