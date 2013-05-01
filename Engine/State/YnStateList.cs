using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.State
{
    /// <summary>
    /// A safe updateable state collection.
    /// </summary>
    public class YnStateList : YnList<YnState>
    {
        /// <summary>
        /// Initialize states
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
        /// Load assets from content manager.
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
        /// Unload content.
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
        /// Logic update for safe states.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="count">Number of states to update</param>
        protected override void DoUpdate(GameTime gameTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw safe states.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        /// <param name="spriteBatch">SpriteBatch object</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int nbSafeMembers = _safeMembers.Count;

            if (nbSafeMembers > 0)
            {
                for (int i = 0; i < nbSafeMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                        _members[i].Draw(gameTime);
                }
            }
        }
    }
}
