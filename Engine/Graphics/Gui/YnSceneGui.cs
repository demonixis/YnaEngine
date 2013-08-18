// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Yna.Engine.Graphics.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Gui.Widgets;

namespace Yna.Engine.Graphics.Scene
{
    /// <summary>
    /// A 2D Scene with a collection of basic objects like timers, a collection of entities (managed with a safe collection) and a gui manager
    /// </summary>
    public class YnSceneGui : YnScene
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

        public YnSceneGui()
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
            base.Clear();
            _guiManager.Clear();
        }

        public override YnBasicEntity GetMemberByName(string name)
        {
            YnBasicEntity basicEntity = base.GetMemberByName(name);

            if (basicEntity == null)
                basicEntity = _guiManager.GetWidgetByName(name);
            
            return basicEntity;
        }

        #endregion
    }
}
