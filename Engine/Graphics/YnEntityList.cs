using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// An entity safe updateable collection
    /// </summary>
    public class YnEntityList : YnList<YnEntity>
    {
        /// <summary>
        /// Initialize entities.
        /// </summary>
        public virtual void Initialize()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].Initialize();
            }
        }

        /// <summary>
        /// Load assets.
        /// </summary>
        public virtual void LoadContent()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].LoadContent();
            }
        }

        /// <summary>
        /// Unload assets.
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
        /// Update logic of safe entities.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="count"></param>
        protected override void DoUpdate(GameTime gameTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw safe entities
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int nbSafeMembers = _safeMembers.Count;

            if (nbSafeMembers > 0)
            {
                for (int i = 0; i < nbSafeMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                        _safeMembers[i].Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
