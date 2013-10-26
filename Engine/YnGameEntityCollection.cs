// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// A collection of updateable and drawable game entities who is safe.
    /// </summary>
    public class YnGameEntityCollection : YnCollection<YnGameEntity>
    {
        /// <summary>
        /// Initialize logic.
        /// </summary>
        public virtual void Initialize()
        {
            for (int i = 0, l = _members.Count; i < l; i++)
                _members[i].Initialize();
        }

        /// <summary>
        /// Load content.
        /// </summary>
        public virtual void LoadContent()
        {
            for (int i = 0, l = _members.Count; i < l; i++)
                _members[i].LoadContent();
        }

        /// <summary>
        /// Unload content.
        /// </summary>
        public virtual void UnloadContent()
        {
            for (int i = 0, l = _members.Count; i < l; i++)
                _members[i].UnloadContent();
        }

        /// <summary>
        /// Update safe members
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0, l = _safeMembers.Count; i < l; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw safe members.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0, l = _safeMembers.Count; i < l; i++)
            {
                if (_safeMembers[i].Visible)
                    _safeMembers[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
