using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna;
using Yna.Utils;

namespace Yna.State
{
    public class YnState : GameState
    {
        #region Private declarations

        protected List<YnObject> _members;
        private List<YnObject> _safeMembers;

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

        #endregion

        #region Properties

        public List<YnObject> Members
        {
            get { return _members; }
        }

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


        /// <summary>
        /// Get or Set the rotation used on the state
        /// </summary>
        public float ScreenRotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Get or Set the zoom factor used on the state
        /// </summary>
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

        #endregion

        /// <summary>
        /// Create a new state that represent a game layer
        /// </summary>
        /// <param name="timeTransitionOn">Transition time on start</param>
        /// <param name="timeTransitionOff">Transition time on end</param>
        public YnState(float timeTransitionOn = 1500f, float timeTransitionOff = 0f) 
            : base (ScreenType.GameState, timeTransitionOn, timeTransitionOff)
        {
            _members = new List<YnObject>();
            _safeMembers = new List<YnObject>();

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
            sceneObject.Initialize();
            _members.Add(sceneObject);
        }

        public void Add(YnObject[] sceneObjects)
        {
            foreach (YnObject sceneObject in sceneObjects)
            {
                sceneObject.LoadContent();
                sceneObject.Initialize();
                _members.Add(sceneObject);
            }
        }

        public void Remove(YnObject sceneObject)
        {
            sceneObject.UnloadContent();
            _members.Remove(sceneObject);
        }

        public void Clear()
        {
            _members.Clear();
            _safeMembers.Clear();
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

            // We make a copy of all screens to provide any error
            // if a screen is removed during the update opreation
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                _safeMembers.Clear();
                _safeMembers.AddRange(_members);

                for (int i = 0; i < nbMembers; i++)
                {
                    if (!_safeMembers[i].Pause)
                        _safeMembers[i].Update(gameTime);
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

            int nbMembers = _safeMembers.Count;

            if (nbMembers > 0)
            {
                spriteBatch.Begin(_spriteSortMode, _blendState, _samplerState, _depthStencilState, _rasterizerState, _effect, _transformMatrix);
                
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                        _safeMembers[i].Draw(gameTime, spriteBatch);
                }

                spriteBatch.End();
            }
        }
    }
}
