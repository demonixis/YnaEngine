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
        private CameraManager _cameraManager;
        private YnScene3D _scene;
        private YnBaseList _basicObjects;

        /// <summary>
        /// Gets (protected sets) the scene.
        /// </summary>
        public YnScene3D Scene
        {
            get { return _scene; }
            protected set { _scene = value; }
        }

        /// <summary>
        /// Gets (protected sets) the active camera.
        /// </summary>
        public BaseCamera Camera
        {
            get { return _cameraManager.GetActiveCamera(); }
            protected set { _cameraManager.SetActiveCamera(value); }
        }

        /// <summary>
        /// Gets (protected sets) the camera manager.
        /// </summary>
        public CameraManager CameraManager
        {
            get { return _cameraManager; }
            protected set { _cameraManager = value; }
        }

        #region Constructors

        /// <summary>
        /// Create a state with a 3D scene and a fixed camera.
        /// </summary>
        public YnState3D()
            : this(new FixedCamera())
        {
            
        }

        /// <summary>
        /// Create a state with a 3D scene and a camera.
        /// </summary>
        /// <param name="camera">Camera to use on this scene.</param>
        public YnState3D(BaseCamera camera)
            : base()
        {
            if (camera != null)
                _cameraManager = new CameraManager(camera);
            else
                _cameraManager = new CameraManager(new FixedCamera());

            _scene = new YnScene3D();
            _basicObjects = new YnBaseList();
            Initialized = false;
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
            _cameraManager.Update(gameTime);
            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _scene.Draw(gameTime, YnG.GraphicsDevice, _cameraManager.GetActiveCamera());
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
        /// Remove an object3D of the scene
        /// </summary>
        /// <param name="object3D"></param>
        public void Remove(YnEntity3D object3D)
        {
            _scene.Remove(object3D);
        }

        /// <summary>
        /// Clear all objects on the state and on the scene
        /// </summary>
        public void Clear()
        {
            _scene.Clear();
        }

        /// <summary>
        /// Add a basic object to the scene.
        /// </summary>
        /// <param name="basicObject">A basic object like Timer, Camera, etc...</param>
        /// <returns>Return true if the object has been added, otherwise return false.</returns>
        public bool Add(YnBase basicObject)
        {
            return _basicObjects.Add(basicObject);
        }

        /// <summary>
        /// Gets a basic member with its index id.
        /// </summary>
        /// <param name="index">Index of object.</param>
        /// <returns>Return desired object if exists, otherwise return null.</returns>
        public YnBase GetBasicMember(int index)
        {
            return _basicObjects[index];
        }

        /// <summary>
        /// Remove a basic object to the scene.
        /// </summary>
        /// <param name="basicObject">Basic object to remove.</param>
        /// <returns>Return true if the object has been succefully removed, otherwise return false.</returns>
        public bool Remove(YnBase basicObject)
        {
            return _basicObjects.Remove(basicObject);
        }

        #endregion
    }
}
