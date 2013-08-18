// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    public class YnPool : YnGameEntity
    {
        private readonly int _maximumPoolSize;
        private YnEntity[] _poolEntities;
        private YnEntity _tempSearchedEntity;

        public YnEntity[] Entities
        {
            get { return _poolEntities; }
        }

        /// <summary>
        /// Gets the size of the pool.
        /// </summary>
        public int Size
        {
            get { return _maximumPoolSize; }
        }

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

        /// <summary>
        /// Create a pool collection with a fixed size. The collection can not be modified.
        /// </summary>
        /// <param name="maxSize">Maximum entity</param>
        public YnPool(int maxSize)
            : base()
        {
            Active = true;
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

        /// <summary>
        /// Gets the first disabled entity index.
        /// </summary>
        /// <returns>Return the index of the first disabled entity in the collection, otherwise return -1.</returns>
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

        /// <summary>
        /// Add a new entity to the pool.
        /// </summary>
        /// <param name="entity">An entity to add.</param>
        /// <returns>Return true if the entity has been added, otherwise return false.</returns>
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
                result = TryReplace(entity);

            return result;
        }

        /// <summary>
        /// Remove a new entity from the pool.
        /// </summary>
        /// <param name="entity">An entity to remove.</param>
        /// <returns>Return true if the entity has been removed, otherwise return false.</returns>
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

        /// <summary>
        /// Try to replace a disabled entity by a newer.
        /// </summary>
        /// <param name="entity">An newer entity to replace.</param>
        /// <returns>Return true if the entity has been replaced, otherwise return false.</returns>
        public bool TryReplace(YnEntity entity)
        {
            int index = GetFirstDisabledEntityIndex();

            if (index > -1)
            {
                _poolEntities[index] = entity;
                return true;
            }

            return false;
        }

        public YnEntity TryRecycle()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clear the pool and sets all elements to null.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null)
                {
                    _poolEntities[i].Active = false;
                    _poolEntities[i] = null;
                }
            }
        }

        /// <summary>
        /// Load entities.
        /// </summary>
        public override void LoadContent()
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
        public override void UnloadContent()
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
        public override void Initialize()
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
        public override void Update(GameTime gameTime)
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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _maximumPoolSize; i++)
            {
                if (_poolEntities[i] != null && _poolEntities[i].Visible)
                    _poolEntities[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
