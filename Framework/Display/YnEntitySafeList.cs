using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display
{
    /// <summary>
    /// Define a safe list for Entities
    /// </summary>
    public class YnEntitySafeList : YnSafeList<YnEntity>
    {
        /// <summary>
        /// Update enabled objects
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _listSize; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Load content of entities if they don't have already loaded
        /// </summary>
        public virtual void LoadContent()
        {
            for (int i = 0; i < _listSize; i++)
            {
                if (!_members[i].AssetLoaded)
                    _members[i].LoadContent();
            }
        }

        /// <summary>
        /// Unload content
        /// </summary>
        public virtual void UnloadContent()
        {
            for (int i = 0; i < _listSize; i++)
            {
                if (_members[i].AssetLoaded)
                    _members[i].UnloadContent();
            }
        }

        /// <summary>
        /// Initialize entities
        /// </summary>
        public virtual void Initialize()
        {
            for (int i = 0; i < _listSize; i++)
                _members[i].Initialize();
        }

        /// <summary>
        /// Draw visible objects
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_listSize > 0)
            {
                for (int i = 0; i < _listSize; i++)
                {
                    if (_members[i].Visible)
                        _members[i].Draw(gameTime, spriteBatch);
                }
            }
        }

        #region Collection methods

        public void AddEntity(YnEntity entity)
        {

        }

        public void AddEntities(YnEntity[] entities)
        {

        }

        public void RemoveEntity(YnEntity entity)
        {

        }

        public void RemoveEntities(YnEntity[] entities)
        {

        }

        public void ClearEntities()
        {

        }

        #endregion
    }
}
