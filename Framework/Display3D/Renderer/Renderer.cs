using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Lighting;

namespace Yna.Framework.Display3D.Renderer
{
    public class Renderer
    {
        public void Draw(BaseCamera camera, YnScene3D scene)
        {
            foreach (YnEntity3D object3D in scene)
            {
                // 1 - Get the good shader

                // 2 - Setup shader

                // 3 - Setup lights
                
                // 4 - Draw model
                object3D.Draw(YnG.GraphicsDevice);
            }
        }
    }
}
