using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display.Gui;

namespace Yna.Framework
{
    /// <summary>
    /// Represent an abstract scene with a collection of basic objects like timers and a gui manager
    /// </summary>
    public abstract class YnScene : YnBase
    {
        #region Protected declarations

        protected YnBaseSafeList _baseList;
        protected YnGui _guiManager;
        protected bool _initialized;
        protected bool _assetsLoaded;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets basic objects
        /// </summary>
        public List<YnBase> BaseObjects
        {
            get { return _baseList.Members; }
            protected set { _baseList.Members = value; }
        }

        /// <summary>
        /// Gets or sets the gui
        /// </summary>
        public YnGui Gui
        {
            get { return _guiManager; }
            protected set { _guiManager = value; }
        }

        #endregion

        public YnScene()
        {
            _baseList = new YnBaseSafeList();
            _guiManager = new YnGui();
            _initialized = false;
            _assetsLoaded = false;
        }

        public virtual void Initialize()
        {
            _baseList.Initialize();
        }

        public abstract void LoadContent();

        public virtual void UnloadContent()
        {
            _baseList.Clear();
            _guiManager.Clear();
            _baseList = null;
            _guiManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            _baseList.Update(gameTime);
        }

        #region Collection methods

        /// <summary>
        /// Add a basic object
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public virtual void Add(YnBase basicObject)
        {
            _baseList.Add(basicObject);
        }

        /// <summary>
        /// Remove a basic object
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public virtual void Remove(YnBase basicObject)
        {
            _baseList.Remove(basicObject);
        }


        /// <summary>
        /// Add a widget to the gui manager
        /// </summary>
        /// <param name="widget"></param>
        public virtual void Add(YnWidget widget)
        {
            _guiManager.Add(widget);
        }

        /// <summary>
        /// Remove a widget from the gui
        /// </summary>
        /// <param name="widget"></param>
        public virtual void Remove(YnWidget widget)
        {
            _guiManager.Remove(widget);
        }

        /// <summary>
        /// Clear basic objects
        /// </summary>
        public virtual void ClearBasicObjects()
        {
            _baseList.Clear();
        }

        /// <summary>
        /// Clear widgets
        /// </summary>
        public virtual void ClearWidgets()
        {
            _guiManager.Clear();
        }

        /// <summary>
        /// Get an YnBase object by its name
        /// </summary>
        /// <param name="name">Name of the object</param>
        /// <returns>An YnBase object or null if don't exists</returns>
        public virtual YnBase GetMemberByName(string name)
        {
            YnBase basicObject = null;

            int baseSize = _baseList.Count;
            int i = 0;
            while (i < baseSize && basicObject == null)
            {
                if (_baseList[i].Name == name)
                    basicObject = _baseList[i];
                i++;
            }

            return basicObject;
        }

        #endregion
    }
}
