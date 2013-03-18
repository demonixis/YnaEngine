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
    public class YnScene3D : YnGroup3D
    {
        protected SceneLight _light;

        /// <summary>
        /// Gets or sets the basic light of the scene.
        /// </summary>
        public SceneLight BasicLight
        {
            get { return _light; }
            set { _light = value; }
        }

        public YnScene3D(BaseCamera camera)
            : base(camera, null)
        {
            _light = new SceneLight();
            _light.AmbientIntensity = 1f;
        }

        public YnScene3D()
            : this(null)
        {

        }

        /// <summary>
        /// Update logic of all members.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Camera.Update(gameTime);
        }

        /// <summary>
        /// Draw members.
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GameTime gameTime, GraphicsDevice device)
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
                            _safeMembers[i].Draw(gameTime, device);
                        }
                    }
                }
            }
        }
    }
}
