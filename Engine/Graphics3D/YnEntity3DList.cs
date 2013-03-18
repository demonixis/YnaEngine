using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A safe collection of 3D entity who can be updated and drawn.
    /// </summary>
    public class YnEntity3DList : YnList<YnEntity3D>
    {
        /// <summary>
        /// Initialize entities.
        /// </summary>
        public virtual void Initialize(BaseCamera camera)
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_members[i].Camera != camera)
                        _members[i].Camera = camera;

                    _members[i].Initialize();
                }
            }
        }

        /// <summary>
        /// Load entities.
        /// </summary>
        public virtual void LoadContent(BaseCamera camera)
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_members[i].Camera != camera)
                        _members[i].Camera = camera;

                    _members[i].LoadContent();
                }
            }
        }

        /// <summary>
        /// Unload entities.
        /// </summary>
        public virtual void UnloadContent()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].UnloadContent();
            }
        }

        /// <summary>
        /// Safe update.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="count">Number of </param>
        protected override void DoUpdate(GameTime gameTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw entities on screen.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="camera"></param>
        /// <param name="light"></param>
        public virtual void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            Draw(gameTime, device, camera, null);
        }

        /// <summary>
        /// Draw entities on screen.
        /// </summary>
        /// <param name="device"></param>
        /// <param name="camera"></param>
        /// <param name="light"></param>
        public virtual void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera, SceneLight light)
        {
            int nbMembers = _safeMembers.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Enabled)
                    {
                        if (light != null)
                            _safeMembers[i].UpdateLighting(light);

                        _safeMembers[i].Draw(gameTime, device);
                    }
                }
            }
        }
    }
}
