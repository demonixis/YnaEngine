using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Yna.Framework.Display;
using Yna.Framework.Display.Gui;
using Yna.Framework.State;

namespace Yna.Framework.Display
{
    public class YnState : BaseState
    {
        #region Private declarations

        protected List<YnObject> _members;
        private List<YnObject> _safeMembers;
        protected YnGui _gui;

        protected bool _assetsLoaded;
        protected bool _initialized;
        protected bool _removeRequest;

        protected SpriteSortMode _spriteSortMode;
        protected BlendState _blendState;
        protected SamplerState _samplerState;
        protected DepthStencilState _depthStencilState;
        protected RasterizerState _rasterizerState;
        protected Effect _effect;

        protected YnTransformer _camera;

        #endregion

        #region Properties

        /// <summary>
        /// Get members attached to the scene
        /// </summary>
        public List<YnObject> Members
        {
            get { return _members; }
        }

        /// <summary>
        /// Get or Set SpriteSortMode
        /// </summary>
        public SpriteSortMode SpriteSortMode
        {
            get { return _spriteSortMode; }
            set { _spriteSortMode = value; }
        }

        /// <summary>
        /// Get or Set BlendState
        /// </summary>
        public BlendState BlendState
        {
            get { return _blendState; }
            set { _blendState = value; }
        }

        /// <summary>
        /// Get or Set SamplerState
        /// </summary>
        public SamplerState SamplerState
        {
            get { return _samplerState; }
            set { _samplerState = value; }
        }

        /// <summary>
        /// Get or Set DepthStencilState
        /// </summary>
        public DepthStencilState DepthStencilState
        {
            get { return _depthStencilState; }
            set { _depthStencilState = value; }
        }

        public YnTransformer Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        #endregion

        #region Constructors

        private YnState()
           : base()
        {
            InitializeDefaultState();
        }

        public YnState(string name, float timeOn, float timeOff)
            : base (name, timeOn, timeOff)
        {
            _name = name;
            InitializeDefaultState();
        }

        public YnState(string name, bool active)
            : this(name, 0, 0)
        {
            Active = active;
        }

        #endregion

        private void InitializeDefaultState()
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
            _camera = new YnTransformer();
        }

        #region Collection methods

        /// <summary>
        /// Add object on the scene
        /// </summary>
        /// <param name="sceneObject"></param>
        public void Add(YnObject sceneObject)
        {
            sceneObject.LoadContent();
            sceneObject.Initialize();
            _members.Add(sceneObject);
        }

        /// <summary>
        /// Add objects on the scene
        /// </summary>
        /// <param name="sceneObjects"></param>
        public void Add(YnObject[] sceneObjects)
        {
            foreach (YnObject sceneObject in sceneObjects)
            {
                sceneObject.LoadContent();
                sceneObject.Initialize();
                _members.Add(sceneObject);
            }
        }

        /// <summary>
        /// Remove an object from the scene
        /// </summary>
        /// <param name="sceneObject"></param>
        public void Remove(YnObject sceneObject)
        {
            sceneObject.UnloadContent();
            _members.Remove(sceneObject);
        }

        /// <summary>
        /// Clear all members on the scene
        /// </summary>
        public void Clear()
        {
            _members.Clear();
            _safeMembers.Clear();
        }

        #endregion

        #region GameState pattern

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
                    if (_safeMembers[i].Enabled)
                        _safeMembers[i].Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            int nbMembers = _safeMembers.Count;

            if (nbMembers > 0)
            {
                spriteBatch.Begin(_spriteSortMode, _blendState, _samplerState, _depthStencilState, _rasterizerState, _effect, _camera.GetTransformMatrix());

                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                        _safeMembers[i].Draw(gameTime, spriteBatch);
                }

                spriteBatch.End();
            }
        }

        // TODO : better algo
        public YnObject GetChildByName(string name)
        {
            YnObject result = null;
            int i = 0;
            while(i < _members.Count && result == null)
            {
                if (_members[i].Name == name)
                    result = _members[i];

                i++;
            }
            return result;
        }
        #endregion
    }
}
