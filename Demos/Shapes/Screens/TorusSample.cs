using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Samples.Screens
{
    public class TorusSample : BaseSample
    {
        YnMeshGeometry torus;

        public TorusSample(string name)
            : base(name)
        {
            torus = new YnMeshGeometry(new TorusGeometry(6, 1, false, 5, 15, Vector3.One, Vector3.Zero), new BasicMaterial("Textures/metal"));
            torus.RotationX = MathHelper.PiOver2;
            torus.TextureRepeat = new Vector2(4);
            Add(torus);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            torus.Position = new Vector3(terrain.Width / 2, 6.5f, terrain.Depth / 2);
        }
    }
}