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
    public class YnScene : YnGroup3D
    {
        // TODO
        // LightManager
        // Camera Manager

        #region Constructors

        public YnScene(BaseCamera camera)
            : base(camera, null)
        {

        }

        public YnScene()
            : this(new FixedCamera())
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Camera.Update(gameTime);
        }

        #endregion
    }
}
