using System;
using System.Collections.Generic;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Yna.Engine.Graphics.Gui.Widgets;

namespace Yna.Engine.Graphics.Scene
{
    /// <summary>
    /// A 2D Scene with a collection of basic objects like timers, a collection of entities (managed with a safe collection) and a gui manager
    /// </summary>
    public class YnSceneGui2D : YnScene2D
    {
        protected YnGui _guiManager;
        protected bool _useOtherBatchForGui;

        /// <summary>
        /// Gets or sets the gui
        /// </summary>
        public YnGui Gui
        {
            get { return _guiManager; }
            protected set { _guiManager = value; }
        }

        /// <summary>
        /// Define if the gui is drawn in a dedicated batch or in the same global batch
        /// </summary>
        public bool UseOtherBatchForGUI
        {
            get { return _useOtherBatchForGui; }
            set { _useOtherBatchForGui = value; }
        }

        public YnSceneGui2D()
            : base()
        {
            _guiManager = new YnGui();
            _useOtherBatchForGui = false;
        }

        /// <summary>
        /// Initialize entities and gui
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _guiManager.Initialize();
        }

        /// <summary>
        /// Load content of entities and Gui
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (!_guiManager.AssetLoaded)
                _guiManager.LoadContent();
        }

        /// <summary>
        /// Unload content of entities and Gui
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            _guiManager.UnloadContent();
        }

        /// <summary>
        /// Update base objects, enabled entities and gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _guiManager.Update(gameTime);
        }

        /// <summary>
        /// Draw all visible entities and the gui (after entities)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (!_useOtherBatchForGui)
                _guiManager.Draw(gameTime, spriteBatch);
        }

        #region Collection methods

        public void Add(YnWidget widget)
        {
            _guiManager.Add(widget);
        }

        public void Remove(YnWidget widget)
        {
            _guiManager.Remove(widget);
        }

        /// <summary>
        /// Clear basic objects, entities and gui
        /// </summary>
        public override void Clear()
        {
            ClearBasicObjects();
            ClearEntities();
            _guiManager.Clear();
        }

        public override YnBasicEntity GetMemberByName(string name)
        {
            YnBasicEntity basicObject = base.GetMemberByName(name);

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
