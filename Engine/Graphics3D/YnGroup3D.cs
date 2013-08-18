// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A container for updating and drawing 3D object
    /// </summary>
    public class YnGroup3D : YnEntity3D
    {
        protected YnEntity3DList _members;

        #region Properties

        /// <summary>
        /// Get the number of elements in the group
        /// </summary>
        public int Count
        {
            get { return _members.Count; }
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
            get { return _members.Members; }
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

        public YnGroup3D(YnEntity3D parent)
        {
            _members = new YnEntity3DList();
            _initialized = false;
            _parent = parent;
        }

        #region Update methods

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

        /// <summary>
        /// Update world and children world matrix.
        /// </summary>
        public override void UpdateMatrix()
        {
            World = Matrix.Identity;

            foreach (YnEntity3D members in _members)
                World *= members.World;
        }

        /// <summary>
        /// Update lights.
        /// </summary>
        /// <param name="light">Light to use.</param>
        public override void UpdateLighting(SceneLight light)
        {
            foreach (YnEntity3D entity3D in _members)
                entity3D.UpdateLighting(light);
        }

        #endregion

        #region GameState Pattern

        /// <summary>
        /// Initialize logic.
        /// </summary>
        public override void Initialize()
        {
            if (!_initialized)
            {
                _members.Initialize();
                _initialized = true;
            }
        }

        /// <summary>
        /// Load content of members.
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetLoaded)
            {
                _members.LoadContent();
                _assetLoaded = true;
            }
        }

        /// <summary>
        /// Unload content of members.
        /// </summary>
        public override void UnloadContent()
        {
            if (_assetLoaded)
            {
                _members.UnloadContent();
                _assetLoaded = false;
            }
        }

        /// <summary>
        /// Update members logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Enabled)
                _members.Update(gameTime);
        }

        /// <summary>
        /// Draw members on screen if visible.
        /// </summary>
        /// <param name="gameTime">GameTime object.</param>
        /// <param name="device">GraphicsDevice object</param>
        public override void Draw(GameTime gameTime, GraphicsDevice device, BaseCamera camera)
        {
            if (Visible)
                _members.Draw(gameTime, device, camera, null);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add an object to the group, the camera used for this group will be used for this object
        /// </summary>
        /// <param name="sceneObject">An object3D</param>
        public virtual bool Add(YnEntity3D sceneObject)
        {
            if (sceneObject is YnScene3D)
                throw new Exception("[YnGroup3D] You can't add a scene on a group, use an YnGroup3D instead");

            if (sceneObject == this)
                throw new Exception("[YnGroup3D] You can't add this group");

            sceneObject.Parent = this;

            if (_assetLoaded)
                sceneObject.LoadContent();

            if (_initialized)
                sceneObject.Initialize();

            return _members.Add(sceneObject);
        }

        /// <summary>
        /// Remove an object of the group
        /// </summary>
        /// <param name="sceneObject"></param>
        public virtual bool Remove(YnEntity3D sceneObject)
        {
            return _members.Remove(sceneObject);
        }

        /// <summary>
        /// Clear the group
        /// </summary>
        public virtual void Clear()
        {
            _members.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnEntity3D member in _members)
                yield return member;
        }

        #endregion
    }
}
