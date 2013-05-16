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
            if (MembersCount > 0)
            {
                for (int i = 0; i < MembersCount; i++)
                    _members[i].Initialize();
            }
        }

        /// <summary>
        /// Load assets.
        /// </summary>
        public virtual void LoadContent()
        {
            if (MembersCount > 0)
            {
                for (int i = 0; i < MembersCount; i++)
                    _members[i].LoadContent();
            }
        }

        /// <summary>
        /// Unload assets.
        /// </summary>
        public virtual void UnloadContent()
        {
            if (MembersCount > 0)
            {
                for (int i = 0; i < MembersCount; i++)
                    _members[i].UnloadContent();
            }
        }

        /// <summary>
        /// Update logic of safe entities.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < SafeMembersCount; i++)
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
            if (SafeMembersCount > 0)
            {
                for (int i = 0; i < SafeMembersCount; i++)
                {
                    if (_safeMembers[i].Visible)
                        _safeMembers[i].Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}
