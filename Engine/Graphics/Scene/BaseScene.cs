using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Gui;

namespace Yna.Engine.Graphics.Scene
{
    /// <summary>
    /// Represent an abstract scene with a collection of basic objects like timers and nothing else
    /// </summary>
    public abstract class BaseScene : YnBase
    {
        protected YnBaseList _baseList;
        protected bool _assetsLoaded;

        /// <summary>
        /// Gets or sets basic objects
        /// </summary>
        public List<YnBase> BaseObjects
        {
            get { return _baseList.Members; }
            protected set { _baseList.Members = value; }
        }

        public bool AssetsLoaded
        {
            get { return _assetsLoaded; }
        }

        public BaseScene()
        {
            _baseList = new YnBaseList();
            _assetsLoaded = false;
        }

        public abstract void Initialize();

        public abstract void LoadContent();

        public virtual void UnloadContent()
        {
            _baseList.Clear();
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
        /// Clear basic objects
        /// </summary>
        public virtual void ClearBasicObjects()
        {
            _baseList.Clear();
        }

        public abstract void Clear();

        /// <summary>
        /// Gets an YnBase object by its name
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
