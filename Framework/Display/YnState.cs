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

        // Collection for the scene
        protected List<YnTimer> _timers;
        protected List<YnBase> _baseObjects;
        protected List<YnEntity> _members;
        protected List<YnEntity> _safeMembers;
        protected YnGui _guiManager;

        // Initialization flags
        protected bool _assetsLoaded;
        protected bool _initialized;
        
        // Clear requests 
        protected bool _clearRequestForTimers;
        protected bool _clearRequestForBasics;
        protected bool _clearRequestForEntities;

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
        /// Gets timers
        /// </summary>
        public List<YnTimer> Timers
        {
            get { return _timers; }
        }

        /// <summary>
        /// Gets basic objects
        /// </summary>
        public List<YnBase> BasicObjects
        {
            get { return _baseObjects; }
        }

        /// <summary>
        /// Gets members attached to the scene
        /// </summary>
        public List<YnEntity> Members
        {
            get { return _members; }
        }

        /// <summary>
        /// Gets or sets the gui manager used for rendering widgets
        /// </summary>
        public YnGui Gui
        {
            get { return _guiManager; }
            set { _guiManager = value; }
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
        }

        public YnState(string name, float timeOn, float timeOff)
            : base(name, timeOn, timeOff)
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

        /// <summary>
        /// Initialize defaut states
        /// </summary>
        private void InitializeDefaultState()
        {
            _timers = new List<YnTimer>();
            _baseObjects = new List<YnBase>();

            _members = new List<YnEntity>();
            _safeMembers = new List<YnEntity>();

            _guiManager = new YnGui();

            _assetsLoaded = false;
            _initialized = false;

            // Clear requests
            _clearRequestForTimers = false;
            _clearRequestForBasics = false;
            _clearRequestForEntities = false;

            _spriteSortMode = SpriteSortMode.Immediate;
            _blendState = BlendState.AlphaBlend;
            _samplerState = SamplerState.LinearClamp;
            _depthStencilState = DepthStencilState.None;
            _rasterizerState = RasterizerState.CullNone;
            _effect = null;

            _camera = new SpriteBatchCamera();
        }

        #region Collection methods

        /// <summary>
        /// Add an entity to the scene
        /// </summary>
        /// <param name="sceneObject"></param>
        public void Add(YnEntity sceneObject)
        {
            sceneObject.LoadContent();
            sceneObject.Initialize();
            _members.Add(sceneObject);
        }

        /// <summary>
        /// Add an array of entity to the scene
        /// </summary>
        /// <param name="sceneObjects"></param>
        public void Add(YnEntity[] sceneObjects)
        {
            foreach (YnEntity sceneObject in sceneObjects)
            {
                sceneObject.LoadContent();
                sceneObject.Initialize();
                _members.Add(sceneObject);
            }
        }

        /// <summary>
        /// Add a timer will be updated automatically, all timers are updated in first
        /// </summary>
        /// <param name="timer">An instance of YnTimer</param>
        public void AddTimer(YnTimer timer)
        {
            _timers.Add(timer);
        }

        /// <summary>
        /// Add a basic object will be updated automatically
        /// </summary>
        /// <param name="basicObject">an instance of YnBase</param>
        public void AddBasicObject(YnBase basicObject)
        {
            _baseObjects.Add(basicObject);
        }

        /// <summary>
        /// Remove an object from the scene
        /// </summary>
        /// <param name="sceneObject"></param>
        public bool Remove(YnEntity sceneObject)
        {
            sceneObject.UnloadContent();
            return _members.Remove(sceneObject);
        }

        /// <summary>
        /// Remove a timer
        /// </summary>
        /// <param name="timer">The timer to remove</param>
        public bool Remove(YnTimer timer)
        {
            return _timers.Remove(timer);
        }

        /// <summary>
        /// Remove a basic object
        /// </summary>
        /// <param name="basicObject"></param>
        /// <returns></returns>
        public bool Remove(YnBase basicObject)
        {
            return _baseObjects.Remove(basicObject);
        }

        /// <summary>
        /// Clear entities, timers and basic objects
        /// </summary>
        public void Clear()
        {
            _clearRequestForTimers = true;
            _clearRequestForBasics = true;
            _clearRequestForEntities = true;
        }

        public void ClearEntities()
        {
            _clearRequestForEntities = true;
        }

        /// <summary>
        /// Clear all timers
        /// </summary>
        public void ClearTimers()
        {
            _clearRequestForTimers = true;
        }

        /// <summary>
        /// Clear all
        /// </summary>
        public void ClearBasicObjects()
        {
            _clearRequestForBasics = true;
        }

        /// <summary>
        /// Get a child by its name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public YnEntity GetEntityByName(string name)
        {
            YnEntity result = null;
            int i = 0;
            int size = _members.Count;
            while (i < size && result == null)
            {
                if (_members[i].Name == name)
                    result = _members[i];

                i++;
            }
            return result;
        }

        /// <summary>
        /// Gets a timer by its name
        /// </summary>
        /// <param name="name">The name of timer</param>
        /// <returns>The timer if it exists then null</returns>
        public YnTimer GetTimerByName(string name)
        {
            YnTimer result = null;
            int i = 0;
            int size = _members.Count;
            while (i < size && result == null)
            {
                if (_timers[i].Name == name)
                    result = _timers[i];

                i++;
            }
            return result;   
        }

        /// <summary>
        /// Gets a basic object by its name
        /// </summary>
        /// <param name="name">The name of the basic object</param>
        /// <returns>The basic object if it exists then null</returns>
        public YnBase GetBaseObjectByName(string name)
        {
            YnBase result = null;
            int i = 0;
            int size = _baseObjects.Count;
            while (i < size && result == null)
            {
                if (_members[i].Name == name)
                    result = _members[i];

                i++;
            }
            return result;
        }

        #endregion

        #region GameState pattern

        public override void Initialize()
        {
            base.Initialize();

            if (!_initialized && _members.Count > 0)
            {
                foreach (YnEntity sceneObject in _members)
                    sceneObject.Initialize();

                _initialized = true;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (!_assetsLoaded && _members.Count > 0)
            {
                foreach (YnEntity sceneObject in _members)
                    sceneObject.LoadContent();

                _assetsLoaded = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            if (_members.Count > 0)
            {
                foreach (YnEntity sceneObject in _members)
                    sceneObject.UnloadContent();
            }

            _members.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check if we muse clear the timer collection
            if (_clearRequestForTimers)
            {
                _timers.Clear();
                _clearRequestForTimers = false;
            }

            if (_clearRequestForBasics)
            {
                _baseObjects.Clear();
                _clearRequestForBasics = false;
            }

            // Check if we must clear the entity collection
            if (_clearRequestForEntities)
            {
                _members.Clear();
                _clearRequestForEntities = false;
            }

            // Update timers
            int timerCount = _timers.Count;
            
            if (timerCount > 0)
            {
                for (int t = 0; t < timerCount; t++)
                    _timers[t].Update(gameTime);
            }

            // Update base objects
            int baseCount = _baseObjects.Count;

            if (baseCount > 0)
            {
                for (int b = 0; b < baseCount; b++)
                    _baseObjects[b].Update(gameTime);
            }

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

            _guiManager.Update(gameTime);
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

            _guiManager.Draw(gameTime, spriteBatch);
        }

        #endregion
    }
}
