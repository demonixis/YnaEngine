using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna;
using Yna.Display;
using Yna.State;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class YnState3D : Screen
    {
        protected BaseCamera _camera;
        private YnScene _scene;
        private List<YnBase3D> _baseMembers;
        private List<YnBase3D> _safeBaseMembers;

        public YnScene Scene
        {
            get { return _scene; }
            protected set { _scene = value; }
        }

        public List<YnBase3D> BaseMembers
        {
            get { return _baseMembers; }
            protected set { _baseMembers = value; }
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
            : base(ScreenType.GameState, 0, 0)
        {
            _camera = new FixedCamera();
            _scene = new YnScene(_camera);
            _baseMembers = new List<YnBase3D>();
            _safeBaseMembers = new List<YnBase3D>();
            Initialized = false;
        }

        public YnState3D(BaseCamera camera)
            : this()
        {
            _camera = camera;
            _scene = new YnScene(_camera);
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
                base.UnloadContent();

                _scene.UnloadContent();

                _scene.Clear();

                Initialized = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            int nbMembers = _baseMembers.Count;

            if (nbMembers > 0)
            {
                _safeBaseMembers.Clear();
                _safeBaseMembers.AddRange(_baseMembers);

                for (int i = 0; i < nbMembers; i++)
                    _safeBaseMembers[i].Update(gameTime);
            }

            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _scene.Draw(YnG.GraphicsDevice);
        }

        #region Collection Management

        /// <summary>
        /// Add an object3D on the scene
        /// </summary>
        /// <param name="object3D">An object3D</param>
        public void Add(YnObject3D object3D)
        {
            _scene.Add(object3D);
        }

        /// <summary>
        /// Add a non drawable object. If it's a Camera it is used as default camera
        /// </summary>
        /// <param name="basic3D">A basic object</param>
        public void Add(YnBase3D basic3D)
        {
            if (basic3D is BaseCamera)
                Camera = (basic3D as BaseCamera);

            _baseMembers.Add(basic3D);
        }

        /// <summary>
        /// Remove an object3D of the scene
        /// </summary>
        /// <param name="object3D"></param>
        public void Remove(YnObject3D object3D)
        {
            _scene.Remove(object3D);
        }

        /// <summary>
        /// Remove a basic object
        /// </summary>
        /// <param name="base3D"></param>
        public void Remove(YnBase3D base3D)
        {
            _baseMembers.Remove(base3D);
        }

        /// <summary>
        /// Clear all objects on the state and on the scene
        /// </summary>
        public void Clear()
        {
            _baseMembers.Clear();
            _safeBaseMembers.Clear();
            _scene.Clear();
        }

        #endregion
    }
}
