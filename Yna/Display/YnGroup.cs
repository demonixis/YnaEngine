﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display
{
    public class YnGroup : YnObject
    {
        private List<YnObject> _members;
        private List<YnObject> _safeMembers;

        private bool _initialized;
        private bool _assetsLoaded;

        /// <summary>
        /// Members of the group
        /// </summary>
        public List<YnObject> Members
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
        public YnObject this[int index]
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

        public YnGroup(int capacity = 0)
        {
            _members = new List<YnObject>(capacity);
            _safeMembers = new List<YnObject>();
            _initialized = false;
            _assetsLoaded = false;
        }

        /// <summary>
        /// Add a new object in the collecion
        /// </summary>
        /// <param name="sceneObject">An object or derivated from YnObject</param>
        public void Add(YnObject sceneObject)
        {
            sceneObject.Parent = this;

            if (_assetsLoaded)
                sceneObject.LoadContent();

            if (_initialized)
                sceneObject.Initialize();

            _members.Add(sceneObject);
        }

        /// <summary>
        /// Add a new object in the collecion
        /// </summary>
        /// <param name="sceneObject">An array of objects or derivated from YnObject</param>
        public void Add(YnObject[] sceneObject)
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

        public void Remove(YnObject sceneObject)
        {
            _members.Remove(sceneObject);
        }

        public void Clear()
        {
            _members.Clear();
            _safeMembers.Clear();
        }

        public override void Initialize()
        {
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                    member.Initialize();
            }

            _initialized = true;
        }

        public override void LoadContent()
        {
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                    member.LoadContent();
            }

            _assetsLoaded = true;
        }

        public override void UnloadContent()
        {
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                    member.UnloadContent();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // We make a copy of all screens to provide any error
            // if a screen is removed during the update opreation
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

        public IEnumerator GetEnumerator()
        {
            foreach (YnObject member in _members)
                yield return member;
        }
    }
}