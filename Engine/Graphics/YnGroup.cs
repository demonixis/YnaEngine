// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// A container of scene object who work as a collection
    /// </summary>
    public class YnGroup : YnEntity
    {
        #region Private declarations

        protected YnGameEntityCollection _entitiesList;
        private bool _initialized;
        private bool _assetsLoaded;

        #endregion

        #region Properties

        /// <summary>
        /// Members of the group
        /// </summary>
        public List<YnGameEntity> Members
        {
            get { return _entitiesList.Members; }
        }

        /// <summary>
        /// The size of the collection
        /// </summary>
        public int Count
        {
            get { return _entitiesList.Count; }
        }

        /// <summary>
        /// Enable of disable the secure cycle. If active, after each update a secure list is created with a copy of current element. 
        /// This list is used for update and draw so you can change the value of the base members safely. If disable this is the base list who are used for
        /// the cycle Update and Draw.
        /// </summary>
        public bool SecureCycle
        {
            get { return _entitiesList.SecureCycle; }
            set { _entitiesList.SecureCycle = value; }
        }

        /// <summary>
        /// Gets or sets the status of asset loading
        /// </summary>
        public bool AssetsLoaded
        {
            get { return _assetsLoaded; }
            set { _assetsLoaded = value; }
        }

        /// <summary>
        /// Gets or sets the position of the group.
        /// Note: The rectangle values are updated
        /// </summary>
        public new Vector2 Position
        {
            get { return _position; }
            set
            {
                Vector2 rawValue = value - _position;

                _position = value;
                _rectangle.X = (int)_position.X;
                _rectangle.Y = (int)_position.Y;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Translate(ref rawValue);
            }
        }

        /// <summary>
        /// Gets or sets the Rectangle (Bounding box) of the group.
        /// Note: The position values are updated
        /// </summary>
        public new Rectangle Rectangle
        {
            get { return _rectangle; }
            set
            {
                _rectangle = value;
                Position = new Vector2(_rectangle.X, _rectangle.Y);
                UpdateRectangle();
            }
        }

        /// <summary>
        /// Gets or sets the position on X.
        /// </summary>
        public new float X
        {
            get { return (int)_position.X; }
            set
            {
                float rawValue = value - _position.X;

                _position.X = value;
                _rectangle.X = (int)value;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Translate(rawValue, 0);

                UpdateRectangle();
            }
        }

        /// <summary>
        /// Gets or sets the position on Y.
        /// </summary>
        public new float Y
        {
            get { return (int)_position.Y; }
            set
            {
                float rawValue = value - _position.Y;

                _position.Y = value;
                _rectangle.Y = (int)value;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Translate(0, rawValue);

                UpdateRectangle();
            }
        }

        /// <summary>
        /// Gets or sets the rotation value for all children. The value is added to the current rotation value of a child. It's not replaced.
        /// </summary>
        public new float Rotation
        {
            get { return _rotation; }
            set
            {
                float rawValue = value - _rotation;

                _rotation = value;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Rotation += rawValue;
            }
        }

        /// <summary>
        /// Gets or sets scale for all children. The value is added to the current scale value of a child. It is not replaced.
        /// </summary>
        public new Vector2 Scale
        {
            get { return _scale; }
            set
            {
                Vector2 rawValue = value - _scale;

                _scale = value;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Scale += rawValue;
            }
        }

        /// <summary>
        /// Ges or sets origin point.
        /// </summary>
        public new Vector2 Origin
        {
            get { return _origin; }
            set 
            {
                Vector2 rawValue = value - _origin;

                _origin = value;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Origin += rawValue;
            }
        }

        /// <summary>
        /// Gets or sets color for all children.
        /// </summary>
        public new Color Color
        {
            get { return _color; }
            set
            {
                _color = value;

                for (int i = 0, l = this.Count; i < l; i++)
                    this[i].Color = value;
            }
        }

        /// <summary>
        /// Gets or sets an element at the specified position
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public YnEntity this[int index]
        {
            get { return _entitiesList[index] as YnEntity; }
            set { _entitiesList[index] = value; }
        }

        #endregion

        #region Constructors

        public YnGroup()
            : this(0)
        {

        }

        public YnGroup(int capacity)
        {
            _entitiesList = new YnGameEntityCollection();
            _initialized = false;
            _assetsLoaded = false;
            _entitiesList.SecureCycle = true;
        }

        public YnGroup(int capacity, int x, int y)
            : this(capacity)
        {
            _position.X = x;
            _position.Y = y;
            _rectangle.X = x;
            _rectangle.Y = y;
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Initialize all members
        /// </summary>
        public override void Initialize()
        {
            _entitiesList.Initialize();
            _initialized = true;
        }

        /// <summary>
        /// Load content of all members
        /// </summary>
        public override void LoadContent()
        {
            _entitiesList.LoadContent();
            _assetsLoaded = true;
            UpdateRectangle();
        }

        /// <summary>
        /// Unload content of all members
        /// </summary>
        public override void UnloadContent()
        {
            _entitiesList.UnloadContent();
        }

        /// <summary>
        /// Update all members
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _entitiesList.Update(gameTime);
        }

        /// <summary>
        /// Draw all members
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _entitiesList.Draw(gameTime, spriteBatch);
        }

        #endregion

        #region Collection methods

        /// <summary>
        /// Add a new object in the collecion
        /// </summary>
        /// <param name="sceneObject">An object or derivated from YnObject</param>
        public void Add(YnEntity sceneObject)
        {
            sceneObject.Parent = this;

            if (_initialized)
                sceneObject.Initialize();

            if (_assetsLoaded)
                sceneObject.LoadContent();
            
            UpdateRectangle();

            _entitiesList.Add(sceneObject);
        }

        /// <summary>
        /// Add a new entity in the group
        /// </summary>
        /// <param name="sceneObject">An array of objects or derivated from YnObject</param>
        public void Add(YnEntity[] sceneObject)
        {
            int size = sceneObject.Length;

            for (int i = 0; i < size; i++)
                Add(sceneObject[i]);
        }

        /// <summary>
        /// Remove an entity from the group
        /// </summary>
        /// <param name="sceneObject"></param>
        public void Remove(YnEntity sceneObject)
        {
            _entitiesList.Remove(sceneObject);

            UpdateRectangle();
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public void Clear()
        {
            _entitiesList.Clear();

            UpdateRectangle();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnEntity member in _entitiesList.Members)
                yield return member;
        }

        public YnEntity GetChildByName(string name)
        {
            YnGameEntity result = null;
            int i = 0;
            while (i < Count && result == null)
            {
                if (Members[i].Name == name)
                    result = Members[i];

                i++;
            }

            return result as YnEntity;
        }

        #endregion

        /// <summary>
        /// Update the size of the group. It's the rectangle that contains all members
        /// </summary>
        public void UpdateRectangle()
        {
            int width = 0;
            int height = 0;

            int size = _entitiesList.Count;

            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    width = Math.Max(width, this[i].Width);
                    height = Math.Max(height, this[i].Height);
                }
            }

            Width = width;
            Height = height;
        }

        public override void Translate(float x, float y)
        {
            base.Translate(x, y);

            for (int i = 0, l = this.Count; i < l; i++)
                this[i].Translate(x, y);
        }

        public override void Move(float x, float y)
        {
            base.Move(x, y);

            for (int i = 0, l = this.Count; i < l; i++)
                this[i].Translate(X - this[i].X, Y - this[i].Y);
        }
    }
}
