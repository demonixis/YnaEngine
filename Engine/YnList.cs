using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics3D;

namespace Yna.Engine
{
    /// <summary>
    /// An updateable "Safe list" where you can safely add, remove and clear items
    /// </summary>
    /// <typeparam name="T">Base class to use</typeparam>
    public abstract class YnList<T>
    {
        protected List<T> _members;
        protected List<T> _safeMembers;
        protected bool _secureCyle;

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

        public YnList()
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
            // We make a copy of all entities to provide any error
            // if an entity is removed during the update operation
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

                DoUpdate(gameTime, nbMembers);
            }
        }

        /// <summary>
        /// Action to make with the safe list
        /// </summary>
        /// <param name="gameTime">GameTime object</param>
        protected abstract void DoUpdate(GameTime gameTime, int count);

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
    }

    /// <summary>
    /// Define a safe list for YnBase objects
    /// </summary>
    public class YnBaseList : YnList<YnBase>
    {
        protected override void DoUpdate(GameTime gameTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_safeMembers[i].Enabled)
                    _safeMembers[i].Update(gameTime);
            }
        }
    }
}
