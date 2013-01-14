using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display
{
    public class YnList : YnEntity
    {
        List<YnEntity> _members;

        public YnList()
        {
            _members = new List<YnEntity>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (YnEntity entity in _members)
                entity.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (YnEntity entity in _members)
                entity.Draw(gameTime, spriteBatch);
        }

        public void Add(YnEntity entity)
        {
            entity.Parent = this;

            if (!entity.AssetLoaded)
            {
                entity.LoadContent();
                entity.Initialize();
            }

            _members.Add(entity);
        }

        public void Remove(YnEntity entity)
        {
            _members.Remove(entity);
        }

        public void Clear()
        {
            _members.Clear();
        }
    }
}
