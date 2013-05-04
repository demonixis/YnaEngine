using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    public class YnPool : IEntity
    {
        private readonly int _maximumPoolSize;
        private YnEntity[] _poolEntities;
        private YnEntity _tempSearchedEntity;

        public YnEntity this[int index]
        {
            get
            {
                if (index < 0 || index >= _maximumPoolSize)
                    throw new IndexOutOfRangeException();
                return _poolEntities[index];
            }
            set
            {
                if (index < 0 || index >= _maximumPoolSize)
                    throw new IndexOutOfRangeException();
                _poolEntities[index] = value;
            }
        }

        public YnPool(int maxSize)
        {
            _maximumPoolSize = maxSize;
            _poolEntities = new YnEntity[maxSize];

            for (int i = 0; i < _maximumPoolSize; i++)
                _poolEntities[i] = null;
        }

        /// <summary>
        /// Gets the first disabled entity.
        /// </summary>
        /// <returns>Return the first disabled entity, otherwise return null.</returns>
        protected YnEntity GetFirstDisabledEntity()
        {
            _tempSearchedEntity = null;
            int i = 0;

            while (i < _maximumPoolSize && _tempSearchedEntity == null)
            {
                _tempSearchedEntity = _poolEntities[i].Enabled ? null : _poolEntities[i];
                i++;
            }

            return _tempSearchedEntity;
        }

        protected int GetFirstDisabledEntityIndex()
        {
            int i = 0;
            int index = -1;

            while (i < _maximumPoolSize && index == -1)
            {
                index = !_poolEntities[i].Enabled ? i : index;
                i++;
            }

            return index;
        }

        /// <summary>
        /// Gets the index of the pool that contains the first null entity.
        /// </summary>
        /// <returns>Return an integer who's the index of the first null entity, otherwise return -1.</returns>
        protected int GetFirstNullIndex()
        {
            int i = 0;
            int index = -1;

            while (i < _maximumPoolSize && index == -1)
            {
                index = _poolEntities[i] == null ? i : index;
                i++;
            }

            return index;
        }

        public bool TryAdd(YnEntity entity)
        {
            bool result = false;
            int validIndex = GetFirstNullIndex();

            if (validIndex > -1)
            {
                _poolEntities[validIndex] = entity;
                result = true;
            }
            else
                result = TryRecycle(entity);

            return result;
        }

        public bool Remove(YnEntity entity)
        {
            int index = System.Array.IndexOf(_poolEntities, entity);

            if (index > -1)
            {
                _poolEntities[index] = null;
                return true;
            }

            return false;
        }

        public bool TryRecycle(YnEntity entity)
        {
            int index = GetFirstDisabledEntityIndex();

            if (index > -1)
            {
                _poolEntities[index] = entity;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Load entities.
        /// </summary>
        public void LoadContent()
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null)
                    _poolEntities[i].LoadContent();
            }
        }

        /// <summary>
        /// Unload entities
        /// </summary>
        public void UnloadContent()
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null)
                    _poolEntities[i].UnloadContent();
            }
        }

        /// <summary>
        /// Initialize entities
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null)
                    _poolEntities[i].Initialize();
            }
        }

        /// <summary>
        /// Update the logic of enabled entities.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null && _poolEntities[i].Enabled)
                    _poolEntities[i].Update(gameTime);
            }
        }


        /// <summary>
        /// Draw visible entities.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null && _poolEntities[i].Visible)
                    _poolEntities[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
