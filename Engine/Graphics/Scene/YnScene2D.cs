using System;
using System.Collections.Generic;
using Yna.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Yna.Engine.Graphics.Scene
{
    /// <summary>
    /// A 2D Scene with a collection of basic objects like timers, a collection of entities (managed with a safe collection) and a gui manager
    /// </summary>
    public class YnScene2D : BaseScene
    {
        protected YnEntityList _entities;

        public List<YnEntity> Entities
        {
            get { return _entities.Members; }
            set { _entities.Members = value; }
        }

        public YnScene2D()
            : base()
        {
            _entities = new YnEntityList();
        }

        /// <summary>
        /// Initialize entities and gui
        /// </summary>
        public override void Initialize()
        {
            _entities.Initialize();
            _initialized = true;
        }

        /// <summary>
        /// Load content of entities and Gui
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetsLoaded && _entities.Count > 0)
            {
                _entities.LoadContent();
                _assetsLoaded = true;
            }
        }

        /// <summary>
        /// Unload content of entities and Gui
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            _entities.UnloadContent();
        }

        /// <summary>
        /// Update base objects, enabled entities and gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _entities.Update(gameTime);
        }

        /// <summary>
        /// Draw all visible entities and the gui (after entities)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _entities.Draw(gameTime, spriteBatch);
        }

        #region Collection methods

        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <param name="reload">True for force reload and initialization</param>
        public void Add(YnEntity entity, bool reload)
        {
            if (_assetsLoaded || reload)
            {
                entity.LoadContent();
                entity.Initialize();
            }

            _entities.Add(entity);
        }

        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="entity">The entity to add</param>
        public void Add(YnEntity entity)
        {
            Add(entity, false);
        }

        /// <summary>
        /// Remove an entity from the scene
        /// </summary>
        /// <param name="entity">The entity to remove</param>
        public void Remove(YnEntity entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Clear all entities from the scene
        /// </summary>
        public void ClearEntities()
        {
            _entities.Clear();
        }

        /// <summary>
        /// Clear basic objects, entities and gui
        /// </summary>
        public override void Clear()
        {
            ClearBasicObjects();
            ClearEntities();
        }

        public override YnBase GetMemberByName(string name)
        {
            YnBase basicObject = base.GetMemberByName(name);

            // try to find it in the Entities collection
            if (basicObject == null)
            {
                int size = _entities.Count;
                int i = 0;

                while (i < size && basicObject == null)
                {
                    if (_entities[i].Name == name)
                        basicObject = _entities[i];
                    i++;
                }
            }

            return basicObject;
        }

        #endregion
    }
}
