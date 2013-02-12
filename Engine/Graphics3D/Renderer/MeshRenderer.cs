using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D.Renderer
{
    public class MeshRenderer
    {
        public void Draw(BaseCamera camera, YnScene3D1 scene)
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
