// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D
{
    // TODO: Fix transforms
    /// <summary>
    /// A container for updating and drawing 3D object
    /// </summary>
    public class YnGroup3D : YnEntity3D
    {
        protected List<YnEntity3D> _members;

        #region Properties

        /// <summary>
        /// Get the number of elements in the group
        /// </summary>
        public int Count => _members.Count;

        /// <summary>
        /// Get the YnObject3D on this scene
        /// </summary>
        public List<YnEntity3D> SceneObjects => _members;

        public YnEntity3D this[int index]
        {
            get
            {
                if (index >= 0 && index < _members.Count)
                    return _members[index];

                return null;
            }
            set
            {
                if (index >= 0 && index < _members.Count)
                    _members[index] = value;
            }
        }

        #endregion

        public YnGroup3D(YnEntity3D parent = null)
        {
            _members = new List<YnEntity3D>();
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

            foreach (YnEntity3D entity in _members)
                _boundingBox = BoundingBox.CreateMerged(_boundingBox, entity.BoundingBox);

            // Update sizes of the scene
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            _boundingSphere.Center = new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2);
            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
        }

        /// <summary>
        /// Update lights.
        /// </summary>
        /// <param name="light">Light to use.</param>
        public override void UpdateLighting(SceneLight light)
        {
            foreach (var entity3D in _members)
                entity3D.UpdateLighting(light);
        }

        #endregion

        #region GameState Pattern

        /// <summary>
        /// Initialize logic.
        /// </summary>
        public override void Initialize()
        {
            if (_initialized)
                return;

            foreach (var member in _members)
                member.Initialize();

            _initialized = true;
        }

        /// <summary>
        /// Load content of members.
        /// </summary>
        public override void LoadContent()
        {
            if (_assetLoaded)
                return;

            foreach (var member in _members)
                member.LoadContent();

            _assetLoaded = true;
        }

        /// <summary>
        /// Unload content of members.
        /// </summary>
        public override void UnloadContent()
        {
            if (!_assetLoaded)
                return;

            foreach (var member in _members)
                member.UnloadContent();

            _assetLoaded = false;
        }

        /// <summary>
        /// Update members logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
                return;

            foreach (var member in _members)
                if (member.Enabled)
                    member.Update(gameTime);
        }

        /// <summary>
        /// Draw members on screen if visible.
        /// </summary>
        /// <param name="gameTime">GameTime object.</param>
        /// <param name="device">GraphicsDevice object</param>
        public override void Draw(GameTime gameTime, GraphicsDevice device, Cameras.Camera camera)
        {
            if (!Visible)
                return;

            foreach (var member in _members)
                if (member.Visible)
                    member.Draw(gameTime, device, camera);
        }

        public void Draw(GameTime gameTime, GraphicsDevice device, Cameras.Camera camera, SceneLight light)
        {
            if (!Visible)
                return;

            foreach (var member in _members)
            {
                if (!member.Visible)
                    continue;

                if (light != null)
                    member.UpdateLighting(light);

                member.Draw(gameTime, device, camera);
            }
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add an object to the group, the camera used for this group will be used for this object
        /// </summary>
        /// <param name="sceneObject">An object3D</param>
        public virtual bool Add(YnEntity3D sceneObject)
        {
            if (sceneObject == this)
                throw new Exception("[YnGroup3D] You can't add this group");

            sceneObject.Parent = this;

            if (_assetLoaded)
                sceneObject.LoadContent();

            if (_initialized)
                sceneObject.Initialize();

            _members.Add(sceneObject);

            return true;
        }

        public virtual YnEntity3D Get(int index)
        {
            if (index >= 0 && index < _members.Count)
                return _members[index];

            return null;
        }

        /// <summary>
        /// Remove an object of the group
        /// </summary>
        /// <param name="sceneObject"></param>
        public virtual bool Remove(YnEntity3D sceneObject) => _members.Remove(sceneObject);

        /// <summary>
        /// Clear the group
        /// </summary>
        public virtual void Clear() => _members.Clear();

        public IEnumerator GetEnumerator()
        {
            foreach (YnEntity3D member in _members)
                yield return member;
        }

        #endregion
    }
}
