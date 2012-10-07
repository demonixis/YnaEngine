using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class YnScene : YnBase3D, IYnUpdatable, IYnDrawable3
    {
        protected List<YnBase3D> _members;
        protected bool _initialized;

        protected BaseCamera _activeCamera;

        #region Properties

        public int Count
        {
            get { return _members.Count; }
        }

        #endregion

        #region Indexer

        public YnBase3D this[int index]
        {
            get
            {
                if (index < 0 || index > _members.Count - 1)
                    return null;
                else
                    return _members[index];
            }
            set
            {
                if (index < 0 || index > _members.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _members[index] = value;
            }
        }

        #endregion

        public YnScene(Game game)
        {
            _members = new List<YnBase3D>();
            _initialized = false;
        }

        #region Update & Draw

        void IYnUpdatable.Update(GameTime gameTime)
        {
            int size = _members.Count;
            if (size > 0)
            {
                foreach (IYnUpdatable sceneObject in _members)
                    sceneObject.Update(gameTime);
            }
        }

        void IYnDrawable3.Draw(GraphicsDevice device)
        {
            int size = _members.Count;
            if (size > 0)
            {
                foreach (IYnDrawable3 sceneObject in _members)
                    sceneObject.Draw(device);
            }
        }

        #endregion

        #region Collection methods

        public void Add(YnBase3D sceneObject)
        {
            _members.Add(sceneObject);
        }

        public void Remove(YnBase3D sceneObject)
        {
            _members.Remove(sceneObject);
        }

        #endregion

        public IEnumerator GetEnumerator()
        {
            foreach (YnBase3D member in _members)
                yield return member;
        }
    }
}
