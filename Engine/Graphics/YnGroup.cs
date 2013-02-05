using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        protected YnEntityList _entitiesList;
        private bool _initialized;
        private bool _assetsLoaded;

        #endregion

        #region Properties

        /// <summary>
        /// Members of the group
        /// </summary>
        public List<YnEntity> Members
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
        /// Gets or sets the status of initialization
        /// True when all objects are initialized
        /// False when initialization has not started yet
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
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
        /// Gets or sets the position of the group and update all children
        /// Note: The rectangle values are updated
        /// </summary>
        public new Vector2 Position
        {
            get { return _position; }
            set
            {
                NormalizePositionType(ref value);

                _position = value;
                _rectangle.X = (int)_position.X;
                _rectangle.Y = (int)_position.Y;

                int nbMembers = _entitiesList.Count;

                if (nbMembers > 0)
                {
                    for (int i = 0; i < nbMembers; i++)
                        _entitiesList[i].Position += value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Rectangle (Bounding box) of the group and update all children
        /// Note: The position values are updated
        /// </summary>
        public new Rectangle Rectangle
        {
            get { return _rectangle; }
            set
            {
                NormalizePositionType(ref value);

                _rectangle = value;

                _position.X = _rectangle.X;
                _position.Y = _rectangle.Y;

                int nbMembers = _entitiesList.Count;

                if (nbMembers > 0)
                {
                    for (int i = 0; i < nbMembers; i++)
                    {
                        if (_entitiesList[i].PositionType == PositionType.Relative)
                            _entitiesList[i].Position += new Vector2(_position.X, _position.Y);
                    }
                }
                
                UpdateRectangle();
            }
        }

        /// <summary>
        /// Gets or sets an element at the specified position
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public YnEntity this[int index]
        {
            get { return _entitiesList[index]; }
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
            _entitiesList = new YnEntityList();
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

            if (_assetsLoaded)
                sceneObject.LoadContent();

            if (_initialized)
                sceneObject.Initialize();

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
            YnEntity result = null;
            int i = 0;
            while (i < Count && result == null)
            {
                if (Members[i].Name == name)
                    result = Members[i];

                i++;
            }
            return result;
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
                    width = Math.Max(width, _entitiesList[i].Width);
                    height = Math.Max(height, _entitiesList[i].Height);
                }
            }

            Width = width;
            Height = height;
        }
    }
}
