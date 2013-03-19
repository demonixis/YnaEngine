using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;
using Yna.Engine.State;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D
{
    public class YnState3D : BaseState
    {
        private BaseCamera _camera;
        private YnScene3D _scene;

        /// <summary>
        /// Gets (protected sets) the scene.
        /// </summary>
        public YnScene3D Scene
        {
            get { return _scene; }
            protected set { _scene = value; }
        }

        /// <summary>
        /// Gets (protected sets) the camera.
        /// </summary>
        public BaseCamera Camera
        {
            get { return _camera; }
            protected set { _camera = value; }
        }

        #region Constructors

        /// <summary>
        /// Create a state with a 3D scene and a fixed camera.
        /// </summary>
        public YnState3D()
            : base()
        {
            _camera = new FixedCamera();
            _scene = new YnScene3D();
            Initialized = false;
        }

        /// <summary>
        /// Create a state with a 3D scene and a camera.
        /// </summary>
        /// <param name="camera">Camera to use on this scene.</param>
        public YnState3D(BaseCamera camera)
            : this()
        {
            if (camera != null)
                _camera = camera;
        }

        /// <summary>
        /// Create a state with a 3D scene and a camera.
        /// </summary>
        /// <param name="name">State name.</param>
        /// <param name="camera">Camera to use on this scene.</param>
        public YnState3D(string name, BaseCamera camera)
            : this(camera)
        {
            _name = name;
        }

        /// <summary>
        /// Create a state with a 3D scene and a fixed camera.
        /// </summary>
        /// <param name="name">State name.</param>
        public YnState3D(string name)
            : this(name, null)
        {

        }

        #endregion

        /// <summary>
        /// Initialize logic of all scene members.
        /// </summary>
        public override void Initialize()
        {
            _scene.Initialize();
        }

        /// <summary>
        /// Load content for all scene members.
        /// </summary>
        public override void LoadContent()
        {
            if (!Initialized)
            {
                base.LoadContent();
                _scene.LoadContent();
                Initialized = true;
            }
        }

        /// <summary>
        /// Unload content of all scene members and clear the scene.
        /// </summary>
        public override void UnloadContent()
        {
            if (Initialized)
            {
                _scene.UnloadContent();
                _scene.Clear();
                Initialized = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _scene.Draw(gameTime, YnG.GraphicsDevice, _camera);
        }

        #region Collection Management

        /// <summary>
        /// Add an object3D on the scene
        /// </summary>
        /// <param name="object3D">An object3D</param>
        public void Add(YnEntity3D object3D)
        {
            _scene.Add(object3D);
        }

        /// <summary>
        /// Add a non drawable object. If it's a Camera it is used as default camera
        /// </summary>
        /// <param name="basic3D">A basic object</param>
        public void Add(YnBase basic)
        {
            if (basic is BaseCamera)
                Camera = (basic as BaseCamera);

            _scene.Add(basic);
        }

        /// <summary>
        /// Remove an object3D of the scene
        /// </summary>
        /// <param name="object3D"></param>
        public void Remove(YnEntity3D object3D)
        {
            _scene.Remove(object3D);
        }

        /// <summary>
        /// Remove a basic object
        /// </summary>
        /// <param name="base3D"></param>
        public void Remove(YnBase basic)
        {
            _scene.Remove(basic);
        }

        /// <summary>
        /// Clear all objects on the state and on the scene
        /// </summary>
        public void Clear()
        {
            _scene.Clear();
        }

        #endregion
    }
}
