using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.State
{
    public class YnStateList : YnList<BaseState>
    {
        public virtual void Initialize()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].Initialize();
            }
        }

        public virtual void LoadContent()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].LoadContent();
            }
        }

        public virtual void UnloadContent()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].UnloadContent();
            }
        }

        protected override void DoUpdate(GameTime gameTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }

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
