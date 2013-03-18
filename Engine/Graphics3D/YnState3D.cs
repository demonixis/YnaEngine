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
        protected BaseCamera _camera;
        private YnScene3D _scene;
        private YnBaseList _basicObjects;

        public YnScene3D Scene
        {
            get { return _scene; }
            protected set { _scene = value; }
        }

        public List<YnBase> BaseMembers
        {
            get { return _basicObjects.Members; }
            protected set { _basicObjects.Members = value; }
        }

        public BaseCamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;
                _scene.Camera = _camera;
            }
        }

        public YnState3D()
            : base()
        {
            _camera = new FixedCamera();
            _scene = new YnScene3D(_camera);
            _basicObjects = new YnBaseList();
            Initialized = false;
        }

        public YnState3D(BaseCamera camera)
            : this()
        {
            _camera = camera;
            _scene = new YnScene3D(_camera);
        }

        public YnState3D(string name, BaseCamera camera)
            : this(camera)
        {
            _name = name;
        }

        public YnState3D(string name)
            : this(name, null)
        {

        }

        public override void LoadContent()
        {
            if (!Initialized)
            {
                base.LoadContent();

                _scene.LoadContent();

                Initialized = true;
            }
        }

        public override void UnloadContent()
        {
            if (Initialized)
            {
                _scene.UnloadContent();

                _scene.Clear();

                Initialized = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            _basicObjects.Update(gameTime);
            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _scene.Draw(gameTime, YnG.GraphicsDevice);
        }

        #region Collection Management

        /// <summary>
        /// Add an object3D on the scene
        /// </summary>
        /// <param name="object3D">An object3D</param>
        public void Add(YnEntity3D object3D)
        {
            object3D.Camera = Camera;
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

            _basicObjects.Add(basic);
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
            _basicObjects.Remove(basic);
        }

        /// <summary>
        /// Clear all objects on the state and on the scene
        /// </summary>
        public void Clear()
        {
            _basicObjects.Clear();
            _scene.Clear();
        }

        #endregion
    }
}
