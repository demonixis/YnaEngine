using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Terrain;
using Yna.Engine.Graphics3D.Controls;

namespace Yna.Samples.Screens
{
    public class MoveObjectSample : YnState3D
    {
        private YnModel alien;

        public MoveObjectSample(string name)
            : base(name)
        {
            FirstPersonCamera camera = new FirstPersonCamera();
            //ThirdPersonCamera camera = new ThirdPersonCamera();
            Add(camera);

            FirstPersonControl control = new FirstPersonControl(camera);
            control.MoveSpeed = 1.25f;
            control.RotationSpeed = 1.45f;
            Add(control);

            alien = new YnModel("Models/Alien/alien1_L");
            alien.Dynamic = true;
            alien.Scale = new Vector3(0.2f);
            //camera.FollowedObject = alien;
            Add(alien);

            //ThirdPersonControl control = new ThirdPersonControl(camera, alien);
            //Add(control);

            SimpleTerrain ground = new SimpleTerrain("Backgrounds/textureMap", 25, 25);
            Add(ground);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Camera.Position = new Vector3(15, 2, 15);
            alien.Position = new Vector3(15, 1, 18);
            int i = 0;
        }
    }
}
