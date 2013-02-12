using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Scene;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D.Scene
{
    public class YnNode : YnList<YnEntity3D>
    {
        protected YnTransform _tranform;
        protected YnNode _parent;

        public YnNode(YnNode parent)
        {
            _parent = parent;
        }

        public virtual void Initialize()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].Initialize();
            }
        }

        public virtual void LoadContent()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].LoadContent();
            }
        }

        public virtual void UnloadContent()
        {
            int nbMembers = _members.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                    _members[i].UnloadContent();
            }
        }

        protected override void DoUpdate(GameTime gameTime, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_members[i].Enabled)
                    _members[i].Update(gameTime);
            }
        }

        public virtual void Draw(GraphicsDevice device, YnBasicLight light)
        {
            int nbMembers = _safeMembers.Count;

            if (nbMembers > 0)
            {
                for (int i = 0; i < nbMembers; i++)
                {
                    if (_safeMembers[i].Visible)
                    {
                        _safeMembers[i].UpdateLighting(light);
                        _safeMembers[i].Draw(device);
                    }
                }
            }
        }
    }
}
