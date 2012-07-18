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

        public void Remove(YnObject ynObject)
        {
            _members.Remove(ynObject);
        }

        public void Clear()
        {
            _members.Clear();
        }

        public override void Initialize() 
        {
            foreach (YnObject member in _members)
                member.Initialize();
        }

        public override void LoadContent()
        {
            foreach (YnObject member in _members)
                member.LoadContent();
        }

        public override void UnloadContent()
        {
            foreach (YnObject member in _members)
               member.UnloadContent();
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
            foreach (YnObject member in _members)
                member.Draw(gameTime, spriteBatch);
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
        
        public IEnumerator GetEnumerator()
        {
        	foreach (YnObject member in _members)
        		yield return member;
        }
    }
}
