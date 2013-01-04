using System;
using System.Collections.Generic;
using Yna.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Yna.Framework.Display
{
    /// <summary>
    /// A 2D Scene with a collection of basic objects like timers, a collection of entities (managed with a safe collection) and a gui manager
    /// </summary>
    public class YnScene2D : YnScene
    {
        protected YnEntitySafeList _entities;
        protected bool _useOtherBatchForGui;

        public List<YnEntity> Entities
        {
            get { return _entities.Members; }
            set { _entities.Members = value; }
        }

        /// <summary>
        /// Define if the gui is drawn in a dedicated batch or in the same global batch
        /// </summary>
        public bool UseOtherBatchForGUI
        {
            get { return _useOtherBatchForGui; }
            set { _useOtherBatchForGui = value; }
        }

        public YnScene2D()
            : base()
        {
            _entities = new YnEntitySafeList();
            _useOtherBatchForGui = false;
        }

        /// <summary>
        /// Initialize entities and gui
        /// </summary>
        public override void Initialize()
        {
            _entities.Initialize();
            _guiManager.Initialize();
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

            _guiManager.LoadContent();
        }

        /// <summary>
        /// Unload content of entities and Gui
        /// </summary>
        public virtual void UnloadContent()
        {
            _entities.UnloadContent();
            _guiManager.UnloadContent();
        }

        /// <summary>
        /// Update base objects, enabled entities and gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _entities.Update(gameTime);

            _guiManager.Update(gameTime);
        }

        /// <summary>
        /// Draw all visible entities and the gui (after entities)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _entities.Draw(gameTime, spriteBatch);

            if (!_useOtherBatchForGui)
                _guiManager.Draw(gameTime, spriteBatch);
        }

        #region Collection methods

        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <param name="reload">True for force reload and initialization</param>
        public void Add(YnEntity entity, bool reload)
        {
            if (!_assetsLoaded || reload)
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
        /// Add an array of entity to the scene
        /// </summary>
        /// <param name="entities">An array of entities to add</param>
        public void Add(YnEntity[] entities)
        {
            Add(entities, false);
        }

        /// <summary>
        /// Add an array of entity to the scene
        /// </summary>
        /// <param name="entities">An array of entities to add</param>
        /// <param name="reload">True for force reload and initialization</param>
        public void Add(YnEntity[] entities, bool reload)
        {
            if (!_assetsLoaded || reload)
            {
                foreach (YnEntity entity in entities)
                {
                    entity.LoadContent();
                    entity.Initialize();
                }
            }

            _entities.Add(entities);
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
        /// Remove an array of entity from the scene
        /// </summary>
        /// <param name="entities">An array of entities</param>
        public void Remove(YnEntity[] entities)
        {
            _entities.Remove(entities);
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
        public void Clear()
        {
            ClearBasicObjects();
            ClearEntities();
            _guiManager.Clear();
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

            // Try to find it in the gui collection
            if (basicObject == null)
            {
                basicObject = _guiManager.GetWidgetByName(name);
            }

            return basicObject;
        }

        #endregion
    }
}
