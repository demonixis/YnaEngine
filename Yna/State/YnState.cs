using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna;
using Yna.State;

namespace Yna.State
{
    public class YnState : GameState
    {
        private List<YnObject> _members;
        private List<YnObject> _dirtyObjects;

        private bool _assetsLoaded;
        private bool _initialized;
        private bool _removeRequest;

        public List<YnObject> Members
        {
            get { return _members; }
        }

        public YnState(float timeTransitionOn = 1500f, float timeTransitionOff = 0f) 
            : base (ScreenType.GameState, timeTransitionOn, timeTransitionOff)
        {
            _members = new List<YnObject>();
            _dirtyObjects = new List<YnObject>();
            _assetsLoaded = false;
            _initialized = false;
            _removeRequest = false;
        }

        public void Add(YnObject sceneObject)
        {
            if (_assetsLoaded)
                sceneObject.LoadContent();

            _members.Add(sceneObject);
        }

        public void Remove(YnObject sceneObject)
        {
            _dirtyObjects.Add(sceneObject);
            _removeRequest = true;
        }

        public override void Initialize() 
        {
            base.Initialize();

            if (!_initialized && _members.Count > 0)
            {
                foreach (YnObject sceneObject in _members)
                    sceneObject.Initialize();

                _initialized = true;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_assetsLoaded && _members.Count > 0)
            {
                foreach (YnObject sceneObject in _members)
                    sceneObject.LoadContent();

                _assetsLoaded = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            if (_members.Count > 0)
            {
                foreach (YnObject sceneObject in _members)
                    sceneObject.UnloadContent();
            }

            _members.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_members.Count > 0)
            {
                foreach (YnObject sceneObject in _members)
                {
                    if (!sceneObject.Pause)
                        sceneObject.Update(gameTime);
                }
            }
        }

        public override void Draw (GameTime gameTime)
		{
            base.Draw(gameTime);

            if (_members.Count > 0)
            {
                spriteBatch.Begin();

                foreach (YnObject sceneObject in _members)
                {
                    if (sceneObject.Visible)
                        sceneObject.Draw(gameTime, spriteBatch);
                }

                spriteBatch.End();

            }

            if (_removeRequest)
                PurgeDirty();
        }

        protected virtual void PurgeDirty()
        {
            if (_dirtyObjects.Count > 0)
            {
                foreach (YnObject dirty in _dirtyObjects)
                    _members.Remove(dirty);

                _dirtyObjects.Clear();
            }

            _removeRequest = true;
        }
    }
}
