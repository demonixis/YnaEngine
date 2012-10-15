using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class YnGroup3D : YnObject3D
    {
        protected List<YnObject3D> _members;
        private List<YnObject3D> _safeMembers;

        #region Properties

        public int Count
        {
            get { return _members.Count; }
        }

        public new BaseCamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;

                foreach (YnObject3D sceneObject in _members)
                    sceneObject.Camera = _camera;
            }
        }

        /// <summary>
        /// Get the YnObject3D on this scene
        /// </summary>
        public List<YnObject3D> SceneObjects
        {
            get { return _members; }
        }

        public YnObject3D this[int index]
        {
            get
            {
                if (index < 0 || index > _members.Count - 1)
                    return null;
                else
                    return _members[index];
            }
            set
            {
                if (index < 0 || index > _members.Count - 1)
                    throw new IndexOutOfRangeException();
                else
                    _members[index] = value;
            }
        }

        #endregion

        #region Constructors

        public YnGroup3D(BaseCamera camera, YnObject3D parent)
        {
            _members = new List<YnObject3D>();
            _safeMembers = new List<YnObject3D>();
            _initialized = false;
            _camera = camera;
            _parent = parent;
        }

        public YnGroup3D(YnObject3D parent)
            : this(new FixedCamera(), parent)
        {
        }

        #endregion

        /// <summary>
        /// Get the bounding box of the scene. All YnObject3D's BoundingBox are updated
        /// </summary>
        /// <returns>The bounding box of the scene</returns>
        public override BoundingBox GetBoundingBox()
        {
            BoundingBox boundingBox = new BoundingBox();

            if (_initialized)
            {
                if (_members.Count > 0)
                {
                    foreach (YnObject3D sceneObject in _members)
                    {
                        BoundingBox box = sceneObject.GetBoundingBox();

                        boundingBox.Min.X = box.Min.X < boundingBox.Min.X ? box.Min.X : boundingBox.Min.X;
                        boundingBox.Min.X = box.Min.Y < boundingBox.Min.Y ? box.Min.Y : boundingBox.Min.Y;
                        boundingBox.Min.X = box.Min.Z < boundingBox.Min.Z ? box.Min.Z : boundingBox.Min.Z;

                        boundingBox.Min.X = box.Min.X < boundingBox.Min.X ? box.Min.X : boundingBox.Min.X;
                        boundingBox.Min.X = box.Min.Y < boundingBox.Min.Y ? box.Min.Y : boundingBox.Min.Y;
                        boundingBox.Min.X = box.Min.Z < boundingBox.Min.Z ? box.Min.Z : boundingBox.Min.Z;
                    }
                }
            }

            // Update sizes of the scene
            _width = boundingBox.Max.X - boundingBox.Min.X;
            _height = boundingBox.Max.Y - boundingBox.Min.Y;
            _depth = boundingBox.Max.Z - boundingBox.Min.Z;

            return boundingBox;
        }

        #region Content Management

        public override void LoadContent()
        {
            if (!_initialized)
            {
                if (_members.Count > 0)
                {
                    foreach (YnObject3D sceneObject in _members)
                    {
                        sceneObject.LoadContent();
                        sceneObject.Camera = _camera;
                    }
                }

                _initialized = true;
            }
        }

        public override void UnloadContent()
        {
            if (_initialized)
            {
                if (_members.Count > 0)
                {
                    foreach (YnObject3D sceneObject in _members)
                        sceneObject.UnloadContent();
                }

                _initialized = false;
            }
        }

        #endregion

        #region Update & Draw

        public override void Update(GameTime gameTime)
        {
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

        public override void Draw(GraphicsDevice device)
        {
            int nbMembers = _safeMembers.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                        _safeMembers[i].Draw(device);
                }
            }
        }

        #endregion

        #region Collection methods

        public void Add(YnObject3D sceneObject)
        {
            if (sceneObject is YnScene)
                throw new Exception("[YnGroup3D] You can't add a scene on a group, use an YnGroup3D instead");

            if (_initialized)
            {
                sceneObject.LoadContent();
                sceneObject.Camera = _camera;
            }

            sceneObject.Parent = this;

            _members.Add(sceneObject);
        }

        public void Remove(YnObject3D sceneObject)
        {
            _members.Remove(sceneObject);
        }

        public void Clear()
        {
            _members.Clear();
            _safeMembers.Clear();
        }

        #endregion

        public IEnumerator GetEnumerator()
        {
            foreach (YnBase3D member in _members)
                yield return member;
        }
    }
}
