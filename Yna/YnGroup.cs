﻿using System;
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

        public List<YnObject> Members
        {
            get { return _members; }
        }
        
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

        public void Add(YnObject ynObject)
        {
            _members.Add(ynObject);
        }

        public void Add(YnObject[] sceneObject)
        {
            int size = sceneObject.Length;
            for (int i = 0; i < size; i++)
            {
                Add(sceneObject[i]);
            }
        }

        public void Remove(YnObject ynObject)
        {
            _members.Remove(ynObject);
        }

        public void Clear()
        {
            _members.Clear();
        }

        public int Count()
        {
            return _members.Count();
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
        
        public void AddX(int value)
        {
    		foreach (YnObject member in _members)
    			member.X += value;
        }
        
        public void AddY(int value)
        {
    		foreach (YnObject member in _members)
    			member.Y += value;
        }

        public void SetScale(float sx, float sy)
        {
            foreach (YnObject member in _members)
                member.Scale = new Vector2(sx, sy);
        }

        public void SetScale(Vector2 scale)
        {
            SetScale(scale.X, Scale.Y);
        }
        
        public IEnumerator GetEnumerator()
        {
        	foreach (YnObject member in _members)
        		yield return member;
        }
    }
}
