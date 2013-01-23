using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display
{
    /// <summary>
    /// A container of scene object who work as a collection
    /// </summary>
    public class YnGroup : YnEntity
    {
        #region Private declarations

        private List<YnEntity> _members;
        private List<YnEntity> _safeMembers;
        private bool _initialized;
        private bool _assetsLoaded;
        private bool _secureCyle;

        #endregion

        #region Properties

        /// <summary>
        /// Members of the group
        /// </summary>
        public List<YnEntity> Members
        {
            get { return _members; }
        }

        /// <summary>
        /// The size of the collection
        /// </summary>
        public int Count
        {
            get { return _members.Count(); }
        }

        /// <summary>
        /// Enable of disable the secure cycle. If active, after each update a secure list is created with a copy of current element. 
        /// This list is used for update and draw so you can change the value of the base members safely. If disable this is the base list who are used for
        /// the cycle Update and Draw.
        /// </summary>
        public bool SecureCycle
        {
            get { return _secureCyle; }
            set { _secureCyle = value; }
        }

        /// <summary>
        /// Get or set the status of initialization
        /// True when all objects are initialized
        /// False when initialization has not started yet
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// Get or set the status of asset loading
        /// </summary>
        public bool AssetsLoaded
        {
            get { return _assetsLoaded; }
            set { _assetsLoaded = value; }
        }

        /// <summary>
        /// Get or Set the [index] element of the collection
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public YnEntity this[int index]
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

        public YnGroup()
            : this(0)
        {

        }

        public YnGroup(int capacity)
        {
            _members = new List<YnEntity>(capacity);
            _safeMembers = new List<YnEntity>();
            _initialized = false;
            _assetsLoaded = false;
            _secureCyle = true;
        }

        public YnGroup (int capacity, int x, int y)
            : this(capacity)
        {
            _position.X = x;
            _position.Y = y;
        }

        #region GameState pattern

        /// <summary>
        /// Initialize all members
        /// </summary>
        public override void Initialize()
        {
            if (_members.Count > 0)
            {
                foreach (YnEntity member in _members)
                    member.Initialize();
            }

            _initialized = true;
        }

        /// <summary>
        /// Load content of all members
        /// </summary>
        public override void LoadContent()
        {
            if (_members.Count > 0)
            {
                foreach (YnEntity member in _members)
                    member.LoadContent();
            }

            _assetsLoaded = true;
        }

        /// <summary>
        /// Unload content of all members
        /// </summary>
        public override void UnloadContent()
        {
            if (_members.Count > 0)
            {
                foreach (YnEntity member in _members)
                    member.UnloadContent();
            }
        }

        /// <summary>
        /// Update all members
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // We make a copy of all screens to provide any error
            // if a screen is removed during the update opreation
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                if (_secureCyle)
                {
                    _safeMembers.Clear();
                    _safeMembers.AddRange(_members);
                }
                else
                {
                    _safeMembers = _members;
                }

                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Enabled)
                        _safeMembers[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draw all members
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int nbMembers = _safeMembers.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                        _safeMembers[i].Draw(gameTime, spriteBatch);
                }
            }
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

            UpdateSizes();

            _members.Add(sceneObject);
        }

        /// <summary>
        /// Add a new entity in the group
        /// </summary>
        /// <param name="sceneObject">An array of objects or derivated from YnObject</param>
        public void Add(YnEntity[] sceneObject)
        {
            int size = sceneObject.Length;
            for (int i = 0; i < size; i++)
            {
                if (_assetsLoaded)
                    sceneObject[i].LoadContent();

                if (_initialized)
                    sceneObject[i].Initialize();

                Add(sceneObject[i]);
            }
        }

        /// <summary>
        /// Remove an entity from the group
        /// </summary>
        /// <param name="sceneObject"></param>
        public void Remove(YnEntity sceneObject)
        {
            _members.Remove(sceneObject);

            UpdateSizes();
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public void Clear()
        {
            _members.Clear();
            _safeMembers.Clear();

            UpdateSizes();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (YnEntity member in _members)
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
        public void UpdateSizes()
        {
            Width = 0;
            Height = 0;

            foreach (YnEntity sceneObject in this)
            {
                Width = Math.Max(Width, sceneObject.Width);
                Height = Math.Max(Height, sceneObject.Height);
            }
        }
    }
}
