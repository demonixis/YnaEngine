using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Scene;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Engine.Graphics3D.Scene
{
    public class Node
    {
        protected YnTransform _tranforms;
        protected Node _parent;

        public Node(Node parent)
        {
            _parent = parent;
            _tranforms = new YnTransform();
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(GraphicsDevice device, BaseCamera camera, YnBasicLight light)
        {
          
        }
    }

}
