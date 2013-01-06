using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Lighting;

namespace Yna.Framework.Display3D
{
    public class YnScene3 : YnEntity3DSafeList
    {

    }

    public class YnScene3D : YnGroup3D
    {
        YnBasicLight _lights;

        #region Constructors

        public YnScene3D(BaseCamera camera)
            : base(camera, null)
        {
            _lights = new YnBasicLight();
        }

        public YnScene3D()
            : this(null)
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Camera.Update(gameTime);
        }

        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);
        }

        #endregion
    }
}
