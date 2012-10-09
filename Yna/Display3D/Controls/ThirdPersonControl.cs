using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Controls
{
    public class ThirdPersonControl : BaseControl
    {
        public ThirdPersonControl(ThirdPersonCamera camera)
            : base(camera)
        {

        }

        public ThirdPersonControl(ThirdPersonCamera camera, PlayerIndex index)
            : base(camera, index)
        {
            
        }
    }
}
