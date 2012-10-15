using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class YnScene : YnGroup3D
    {
        #region Constructors

        public YnScene(BaseCamera camera)
            : base(camera, null)
        {

        }

        public YnScene()
            : this(new FixedCamera())
        {

        }

        #endregion
    }
}
