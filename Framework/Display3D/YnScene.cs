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
    public class YnScene3D : YnGroup3D
    {
        #region Constructors

        protected YnBasicLight _light;

        public YnBasicLight BasicLight
        {
            get { return _light; }
            set { _light = value; }
        }

        public YnScene3D(BaseCamera camera)
            : base(camera, null)
        {
            _light = new YnBasicLight();
            _light.AmbientIntensity = 1f;
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
            if (Visible)
            {
                int nbMembers = _safeMembers.Count;

                if (nbMembers > 0)
                {
                    for (int i = 0; i < nbMembers; i++)
                    {
                        if (_safeMembers[i].Visible)
                        {
                            _safeMembers[i].UpdateLighting(_light);
                            _safeMembers[i].Draw(device);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
