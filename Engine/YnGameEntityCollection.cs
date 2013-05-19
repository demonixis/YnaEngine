using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    public class YnGameEntityCollection : YnCollection<YnGameEntity>
    {
        public virtual void Initialize()
        {
            for (int i = 0; i < MembersCount; i++)
                _members[i].Initialize();
        }

        public virtual void LoadContent()
        {
            for (int i = 0; i < MembersCount; i++)
                _members[i].LoadContent();
        }

        public virtual void UnloadContent()
        {
            for (int i = 0; i < MembersCount; i++)
                _members[i].UnloadContent();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < SafeMembersCount; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < SafeMembersCount; i++)
            {
                if (_safeMembers[i].Visible)
                    _safeMembers[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
