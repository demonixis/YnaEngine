using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    public class YnScene3D1 : YnGroup3D
    {
        #region Constructors

        protected YnBasicLight _light;

        public YnBasicLight BasicLight
        {
            get { return _light; }
            set { _light = value; }
        }

        public YnScene3D1(BaseCamera camera)
            : base(camera, null)
        {
            _light = new YnBasicLight();
            _light.AmbientIntensity = 1f;
        }

        public YnScene3D1()
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
