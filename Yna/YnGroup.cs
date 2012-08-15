using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna
{
    public class YnGroup : YnObject
    {
        private List<YnObject> _members;

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
        }

        /// <summary>
        /// Add a new object in the collecion
        /// </summary>
        /// <param name="sceneObject">An object or derivated from YnObject</param>
        public void Add(YnObject sceneObject)
        {
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
        }

        public override void Initialize() 
        {
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                    member.Initialize();
            }
        }

        public override void LoadContent()
        {
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                    member.LoadContent();
            }
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
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                {
                    if (!member.Pause)
                        member.Update(gameTime);
                }

                PostUpdate();
            }
        }

        protected virtual void PostUpdate()
        {
            int size = _members.Count;
            
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (_members[i].Dirty)
                        _members.Remove(_members[i]);
                }
            }
        }

        public override void Draw (GameTime gameTime, SpriteBatch spriteBatch)
		{
            if (_members.Count > 0)
            {
                foreach (YnObject member in _members)
                    member.Draw(gameTime, spriteBatch);
            }
        }
        
        /// <summary>
        /// Add the value on X property of all members of the group
        /// </summary>
        /// <param name="value">Value to add</param>
        public void AddX(int value)
        {
    		foreach (YnObject member in _members)
    			member.X += value;
        }

        /// <summary>
        /// Add the value on Y property of all members of the group
        /// </summary>
        /// <param name="value">Value to add</param>
        public void AddY(int value)
        {
    		foreach (YnObject member in _members)
    			member.Y += value;
        }

        /// <summary>
        /// Set scale on all members of the group
        /// </summary>
        /// <param name="sx"></param>
        /// <param name="sy"></param>
        public void SetScale(float sx, float sy)
        {
            foreach (YnObject member in _members)
                member.Scale = new Vector2(sx, sy);
        }
        
        public IEnumerator GetEnumerator()
        {
        	foreach (YnObject member in _members)
        		yield return member;
        }
    }
}
