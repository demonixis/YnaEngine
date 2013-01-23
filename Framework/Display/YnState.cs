using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Yna.Framework.Display;
using Yna.Framework.Display.Gui;
using Yna.Framework.State;

namespace Yna.Framework.Display
{
    /// <summary>
    /// This is a State object that contains the scene.
    /// That allows you to add different types of objects.
    /// Timers, basic objects (which have an update method) and entities
    /// </summary>
    public class YnState : BaseState
    {
        #region Private declarations

        // The scene
        YnScene2D _scene;

        // SpriteBatch modes
        protected SpriteSortMode _spriteSortMode;
        protected BlendState _blendState;
        protected SamplerState _samplerState;
        protected DepthStencilState _depthStencilState;
        protected RasterizerState _rasterizerState;
        protected Effect _effect;
        
        // SpriteBatch transform camera
        protected SpriteBatchCamera _camera;

        #endregion

        #region Properties

        /// <summary>
        /// Gets basic objects
        /// </summary>
        public List<YnBase> BasicObjects
        {
            get { return _scene.BaseObjects; }
        }

        /// <summary>
        /// Gets members attached to the scene
        /// </summary>
        public List<YnEntity> Entities
        {
            get { return _scene.Entities; }
        }

        /// <summary>
        /// Gets or sets the gui manager used for rendering widgets
        /// </summary>
        public YnGui Gui
        {
            get { return _scene.Gui; }
        }

        /// <summary>
        /// Gets or sets SpriteSortMode
        /// </summary>
        public SpriteSortMode SpriteSortMode
        {
            get { return _spriteSortMode; }
            set { _spriteSortMode = value; }
        }

        /// <summary>
        /// Gets or sets BlendState
        /// </summary>
        public BlendState BlendState
        {
            get { return _blendState; }
            set { _blendState = value; }
        }

        /// <summary>
        /// Gets or sets SamplerState
        /// </summary>
        public SamplerState SamplerState
        {
            get { return _samplerState; }
            set { _samplerState = value; }
        }

        /// <summary>
        /// Gets or sets DepthStencilState
        /// </summary>
        public DepthStencilState DepthStencilState
        {
            get { return _depthStencilState; }
            set { _depthStencilState = value; }
        }

        /// <summary>
        /// Gets or sets the spriteBatchCamera used for add effect on the scene like 
        /// displacement, rotation and zoom
        /// </summary>
        public SpriteBatchCamera Camera
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
            _scene = new YnScene2D();
        }

        public YnState(string name, float timeOn, float timeOff)
            : base(name, timeOn, timeOff)
        {
            _name = name;
            InitializeDefaultState();
            _scene = new YnScene2D();
        }

        public YnState(string name, bool active)
            : this(name, 0, 0)
        {
            Active = active;
        }

        #endregion

        /// <summary>
        /// Initialize defaut states
        /// </summary>
        private void InitializeDefaultState()
        {
            _spriteSortMode = SpriteSortMode.Immediate;
            _blendState = BlendState.AlphaBlend;
            _samplerState = SamplerState.LinearClamp;
            _depthStencilState = DepthStencilState.None;
            _rasterizerState = RasterizerState.CullNone;
            _effect = null;

            _camera = new SpriteBatchCamera();
        }

        #region GameState pattern

        /// <summary>
        /// Initialize the state
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _scene.Initialize();
        }

        /// <summary>
        /// Load content
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            _scene.LoadContent();
        }

        /// <summary>
        /// Unload content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();

            _scene.UnloadContent();
        }

        /// <summary>
        /// Update the camera and the scene who will update BasicObjects, Entities and Gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _camera.Update(gameTime);

            _scene.Update(gameTime);
        }

        /// <summary>
        /// Draw all entities and the gui
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            int nbMembers = _scene.Entities.Count;
            
            if (!_scene.UseOtherBatchForGUI)
                nbMembers += _scene.Gui.Count;

            if (nbMembers > 0)
            {
                spriteBatch.Begin(_spriteSortMode, _blendState, _samplerState, _depthStencilState, _rasterizerState, _effect, _camera.GetTransformMatrix());

                _scene.Draw(gameTime, spriteBatch);

                spriteBatch.End();
            }

            if (_scene.UseOtherBatchForGUI)
            {
                spriteBatch.Begin(_spriteSortMode, _blendState, _samplerState, _depthStencilState, _rasterizerState, _effect, _camera.GetTransformMatrix());
                _scene.Gui.Draw(gameTime, spriteBatch);
                spriteBatch.End();
            }
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Add(YnBase basicObject)
        {
            _scene.Add(basicObject);
        }

        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="entity">An entitiy</param>
        public void Add(YnEntity entity)
        {
            _scene.Add(entity);
        }

        /// <summary>
        /// Add a widget to the scene
        /// </summary>
        /// <param name="widget">A widget</param>
        public void Add(YnWidget widget)
        {
            _scene.Add(widget);
        }

        /// <summary>
        /// Remove a basic object to the scene
        /// </summary>
        /// <param name="basicObject">A basic object</param>
        public void Remove(YnBase basicObject)
        {
            _scene.Remove(basicObject);
        }

        /// <summary>
        /// Remove an entity to the scene
        /// </summary>
        /// <param name="entity">An entitiy</param>
        public void Remove(YnEntity entity)
        {
            _scene.Remove(entity);
        }

        /// <summary>
        /// Remove a widget to the scene
        /// </summary>
        /// <param name="widget">A widget</param>
        public void Remove(YnWidget widget)
        {
            _scene.Remove(widget);
        }

        public YnBase GetMemberByName(string name)
        {
            return _scene.GetMemberByName(name);
        }

        #endregion
    }
}
