using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    // @Deprecated
    public class YnGroup3D : YnEntity3D
    {
        protected List<YnEntity3D> _members;
        protected List<YnEntity3D> _safeMembers;

        #region Properties

        /// <summary>
        /// Get the number of elements in the group
        /// </summary>
        public int Count
        {
            get { return _members.Count; }
        }

        /// <summary>
        /// Get or Set the camera used for this group. If set all objects are updated with this camera
        /// </summary>
        public new BaseCamera Camera
        {
            get { return _camera; }
            set
            {
                _camera = value;

                foreach (YnEntity3D sceneObject in _members)
                    sceneObject.Camera = _camera;
            }
        }

        public new Matrix World
        {
            get
            {
                _world = Matrix.Identity;

                foreach (YnEntity3D members in _members)
                    _world *= members.World;

                return _world;
            }
            set
            {
                foreach (YnEntity3D members in _members)
                    members.World *= value;
            }
        }

        /// <summary>
        /// Get the YnObject3D on this scene
        /// </summary>
        public List<YnEntity3D> SceneObjects
        {
            get { return _members; }
        }

        public YnEntity3D this[int index]
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

        public new Vector3 Rotation
        {
            get { return _rotation; }
            set
            {
                foreach (YnEntity3D entity in this)
                    entity.Rotation += value;
                _rotation = value;
            }
        }

        public new Vector3 Position
        {
            get { return _position; }
            set
            {
                foreach (YnEntity3D entity in this)
                    entity.Position += value;
                _position = value;
            }
        }

        public new Vector3 Scale
        {
            get { return _scale; }
            set
            {
                foreach (YnEntity3D entity in this)
                    entity.Scale += value;
                _scale = value;
            }
        }

        #endregion

        #region Constructors

        public YnGroup3D(BaseCamera camera, YnEntity3D parent)
        {
            _members = new List<YnEntity3D>();
            _safeMembers = new List<YnEntity3D>();
            _initialized = false;
            _camera = camera;
            _parent = parent;
        }

        public YnGroup3D(YnEntity3D parent)
            : this(new FixedCamera(), parent)
        {
        }

        #endregion

        #region Compute bounding box & bounding sphere

        /// <summary>
        /// Get the bounding box of the scene. All YnObject3D's BoundingBox are updated
        /// </summary>
        /// <returns>The bounding box of the scene</returns>
        public override void UpdateBoundingVolumes()
        {
            _boundingBox = new BoundingBox();

            if (_initialized)
            {
                if (_members.Count > 0)
                {
                    foreach (YnEntity3D sceneObject in _members)
                    {
                        BoundingBox box = sceneObject.BoundingBox;

                        _boundingBox.Min.X = box.Min.X < _boundingBox.Min.X ? box.Min.X : _boundingBox.Min.X;
                        _boundingBox.Min.X = box.Min.Y < _boundingBox.Min.Y ? box.Min.Y : _boundingBox.Min.Y;
                        _boundingBox.Min.X = box.Min.Z < _boundingBox.Min.Z ? box.Min.Z : _boundingBox.Min.Z;

                        _boundingBox.Min.X = box.Min.X < _boundingBox.Min.X ? box.Min.X : _boundingBox.Min.X;
                        _boundingBox.Min.X = box.Min.Y < _boundingBox.Min.Y ? box.Min.Y : _boundingBox.Min.Y;
                        _boundingBox.Min.X = box.Min.Z < _boundingBox.Min.Z ? box.Min.Z : _boundingBox.Min.Z;
                    }
                }
            }

            // Update sizes of the scene
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            _boundingSphere.Center = new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;

            World = Matrix.Identity;

            foreach (YnEntity3D members in _members)
                World *= members.World;
        }

        #endregion

        public override void UpdateMatrix()
        {
            World = Matrix.Identity;

            foreach (YnEntity3D members in _members)
                World *= members.World;
        }

        public override void UpdateLighting(SceneLight light)
        {
            foreach (YnEntity3D entity3D in _members)
                entity3D.UpdateLighting(light);
        }

        #region GameState Pattern

        public override void LoadContent()
        {
            if (!_initialized)
            {
                if (_members.Count > 0)
                {
                    foreach (YnEntity3D sceneObject in _members)
                    {
                        sceneObject.Camera = _camera;
                        sceneObject.LoadContent();
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
                    foreach (YnEntity3D sceneObject in _members)
                        sceneObject.UnloadContent();
                }

                _initialized = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Enabled)
            {
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
        }

        public override void Draw(GraphicsDevice device)
        {
            if (Visible)
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
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add an object to the group, the camera used for this group will be used for this object
        /// </summary>
        /// <param name="sceneObject">An object3D</param>
        public void Add(YnEntity3D sceneObject)
        {
            if (sceneObject is YnScene3D1)
                throw new Exception("[YnGroup3D] You can't add a scene on a group, use an YnGroup3D instead");

            if (sceneObject == this)
                throw new Exception("[YnGroup3D] You can't add this group");

            if (_initialized)
            {
                sceneObject.Camera = _camera;
                sceneObject.LoadContent();
            }

            sceneObject.Parent = this;

            _members.Add(sceneObject);
        }

        /// <summary>
        /// Remove an object of the group
        /// </summary>
        /// <param name="sceneObject"></param>
        public void Remove(YnEntity3D sceneObject)
        {
            _members.Remove(sceneObject);
        }

        /// <summary>
        /// Clear the group
        /// </summary>
        public void Clear()
        {
            _members.Clear();
            _safeMembers.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnBase3D member in _members)
                yield return member;
        }

        #endregion
    }
}
