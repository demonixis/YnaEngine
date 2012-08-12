using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna;

namespace Yna.State
{
    public class YnState : GameState
    {
        protected List<YnObject> _members;
        protected List<YnObject> _dirtyObjects;

        protected bool _assetsLoaded;
        protected bool _initialized;
        protected bool _removeRequest;

        protected SpriteSortMode _spriteSortMode;
        protected BlendState _blendState;
        protected SamplerState _samplerState;
        protected DepthStencilState _depthStencilState;
        protected RasterizerState _rasterizerState;
        protected Effect _effect;
        protected Matrix _transformMatrix;
        protected float _rotation;
        protected float _zoom;

        public SpriteSortMode SpriteSortMode
        {
            get { return _spriteSortMode; }
            set { _spriteSortMode = value; }
        }

        public BlendState BlendState
        {
            get { return _blendState; }
            set { _blendState = value; }
        }

        public SamplerState SamplerState
        {
            get { return _samplerState; }
            set { _samplerState = value; }
        }

        public DepthStencilState DepthStencilState
        {
            get { return _depthStencilState; }
            set { _depthStencilState = value; }
        }

        public float ScreenRotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public float ScreenZoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;

                if (_zoom < 0)
                    _zoom = 0;
            }
        }

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

            _spriteSortMode = SpriteSortMode.Immediate;
            _blendState = BlendState.AlphaBlend;
            _samplerState = SamplerState.LinearClamp;
            _depthStencilState = DepthStencilState.None;
            _rasterizerState = RasterizerState.CullNone;
            _effect = null;
            _transformMatrix = Matrix.Identity;

            _rotation = 0.0f;
            _zoom = 1.0f;
        }

        public void Add(YnObject sceneObject)
        {
            sceneObject.LoadContent();
            _members.Add(sceneObject);
        }

        public void Add(YnObject[] sceneObjects)
        {
            foreach (YnObject sceneObject in sceneObjects)
            {
                sceneObject.LoadContent();
                _members.Add(sceneObject);
            }
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

        protected Matrix GetTransformMatrix()
        {
            Matrix translateToOrigin = Matrix.CreateTranslation(-YnG.Width / 2, -YnG.Height / 2, 0);
            Matrix rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(_rotation));
            Matrix zoom = Matrix.CreateScale(_zoom);
            Matrix translateBackToPosition = Matrix.CreateTranslation(YnG.Width / 2, YnG.Height / 2, 0);
            Matrix composition = translateToOrigin * rotation * zoom * translateBackToPosition;

            return composition;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _transformMatrix = GetTransformMatrix();

            if (_members.Count > 0)
            {
                spriteBatch.Begin(_spriteSortMode, _blendState, _samplerState, _depthStencilState, _rasterizerState, _effect, _transformMatrix);

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
