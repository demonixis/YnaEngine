using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display;
using Yna.Framework.Display3D;

namespace Yna.Framework
{
    /// <summary>
    /// An updateable "Safe list" where you can safely add, remove and clear items
    /// </summary>
    /// <typeparam name="T">Base class to use</typeparam>
    public abstract class YnSafeList<T>
    {
        protected List<T> _members;
        protected List<T> _membersToAdd;
        protected List<T> _membersToRemove;
        protected T[] _safeMembers;
        protected int _listSize;
        protected bool _addRequest;
        protected bool _clearRequest;
        protected bool _removeRequest;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > _members.Count)
                    throw new IndexOutOfRangeException();

                return _members[index];
            }
            set
            {
                if (index < 0 || index > _members.Count)
                    throw new IndexOutOfRangeException();

                _members[index] = value;
            }
        }

        /// <summary>
        /// Gets or sets the members of the list.
        /// </summary>
        public List<T> Members
        {
            get { return _members; }
            set { _members = value; }
        }

        /// <summary>
        /// Gets the number of object in the collection
        /// </summary>
        public int Count
        {
            get { return _members.Count; }
        }

        public YnSafeList()
        {
            _members = new List<T>();
            _membersToAdd = new List<T>();
            _membersToRemove = new List<T>();
            _safeMembers = null;
            _listSize = 0;
            _addRequest = false;
            _clearRequest = false;
            _removeRequest = false;
        }

        /// <summary>
        /// Add items that are ready to the collection
        /// </summary>
        public virtual void Initialize()
        {
            CheckForAdd();
        }

        /// <summary>
        /// Check if objects must be added to the list
        /// </summary>
        protected virtual void CheckForAdd()
        {
            if (_addRequest)
            {
                _members.AddRange(_membersToAdd);
                _listSize = _members.Count;
                _membersToAdd.Clear();
            }

            _addRequest = false;
        }

        /// <summary>
        /// Check if the list must be cleared
        /// </summary>
        protected virtual void CheckForClear()
        {
            if (_clearRequest)
            {
                _members.Clear();
                _membersToRemove.Clear();
                _listSize = 0;
            }

            _removeRequest = false;
            _clearRequest = false;
        }

        /// <summary>
        /// Check if objects must be removed from the list
        /// </summary>
        protected virtual void CheckForRemove()
        {
            if (_removeRequest)
            {
                foreach (T item in _membersToRemove)
                    _members.Remove(item);

                _membersToRemove.Clear();
                _listSize = _members.Count;
            }

            _removeRequest = false;
        }

        /// <summary>
        /// Update the collection. Objects are added, removed or cleard here
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public virtual void Update(GameTime gameTime)
        {
            CheckForClear();
            CheckForRemove();
            CheckForAdd();

            int size = _members.Count;

            if (size > 0)
            {
                _safeMembers = null;
                _safeMembers = new T[size];
                _listSize = size;

                for (int i = 0; i < size; i++)
                    _safeMembers[i] = _members[i];

                DoUpdate(gameTime);
            }
        }

        /// <summary>
        /// Action to make with the safe list
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        protected abstract void DoUpdate(GameTime gameTime);

        /// <summary>
        /// Add an item in the collection
        /// </summary>
        /// <param name="item">An item</param>
        public virtual void Add(T item)
        {
            _membersToAdd.Add(item);
            _addRequest = true;
        }

        /// <summary>
        /// Add an array of items in the collection
        /// </summary>
        /// <param name="items">An array of items</param>
        public virtual void Add(T[] items)
        {
            foreach (T item in items)
                _membersToAdd.Add(item);

            _addRequest = true;
        }

        /// <summary>
        /// Force the collection to add the item now
        /// </summary>
        /// <param name="item">Item to add</param>
        public virtual void ForceAdd(T item)
        {
            _members.Add(item);
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="item">Item</param>
        public virtual void Remove(T item)
        {
            _membersToRemove.Add(item);
            _removeRequest = true;
        }

        /// <summary>
        /// Remove a range of items from the collection
        /// </summary>
        /// <param name="items">An array of items</param>
        public virtual void Remove(T[] items)
        {
            foreach (T item in items)
                _membersToRemove.Add(item);

            _removeRequest = true;
        }

        /// <summary>
        /// Force the collection to remove the item now
        /// </summary>
        /// <param name="item">Item to remove</param>
        public virtual void ForceRemove(T item)
        {
            _members.Remove(item);
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public virtual void Clear()
        {
            _clearRequest = true;
        }

        /// <summary>
        /// Force the collection to clear now
        /// </summary>
        public virtual void ForceClear()
        {
            Clear();
            CheckForClear();
        }
    }

    /// <summary>
    /// Define a safe list for YnBase objects
    /// </summary>
    public class YnBaseSafeList : YnSafeList<YnBase>
    {
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _listSize; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }
    }

    /// <summary>
    /// Define a safe list for 3D objects
    /// </summary>
    public class YnObject3DSafeList : YnSafeList<YnObject3D>
    {
        /// <summary>
        /// Update enabled objects
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void DoUpdate(GameTime gameTime)
        {
            for (int i = 0; i < _listSize; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Draw the visible objects
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_listSize > 0)
            {
                for (int i = 0; i < _listSize; i++)
                {
                    if (_members[i].Visible)
                        _members[i].Draw(YnG.GraphicsDevice);
                }
            }
        }
    }
}
