// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    /// <summary>
    /// An updateable "Safe list" where you can safely add, remove and clear items
    /// </summary>
    /// <typeparam name="T">Base class to use</typeparam>
    public abstract class YnCollection<T>
    {
        protected List<T> _members;
        protected List<T> _safeMembers;
        private bool _secureCyle;
        private int _safeMembersCount;

        /// <summary>
        /// Enable or disable the secure cycle. If enabled the collection must be updated to get all enabled members.
        /// </summary>
        public bool SecureCycle
        {
            get { return _secureCyle; }
            set { _secureCyle = value; }
        }

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

        public YnCollection()
        {
            _members = new List<T>();
            _safeMembers = new List<T>();
            _secureCyle = false;
        }

        /// <summary>
        /// Update the collection. Objects are added, removed or cleard here
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        public virtual void Update(GameTime gameTime)
        {
            if (_members.Count > 0)
            {
                if (_secureCyle)
                {
                    _safeMembers.Clear();
                    _safeMembers.AddRange(_members);
                }
                else
                    _safeMembers = _members;

                _safeMembersCount = _safeMembers.Count;

                DoUpdate(gameTime);
            }
        }

        /// <summary>
        /// Action to make with the safe list
        /// </summary>
        protected abstract void DoUpdate(GameTime gameTime);

        /// <summary>
        /// Add an item in the collection
        /// </summary>
        /// <param name="item">An item</param>
        public virtual bool Add(T item)
        {
            if (_members.Contains(item))
                return false;

            _members.Add(item);
            return true;
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="item">Item</param>
        public virtual bool Remove(T item)
        {
            if (!_members.Contains(item))
                return false;

            _members.Remove(item);
            return true;
        }

        /// <summary>
        /// Clear the collection
        /// </summary>
        public virtual void Clear()
        {
            _members.Clear();
        }

        public virtual int IndexOf(T element)
        {
            return _members.IndexOf(element);
        }

        public virtual bool Contains(T element)
        {
            return _members.Contains(element);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (T t in _members)
                yield return t; 
        }
    }
}
