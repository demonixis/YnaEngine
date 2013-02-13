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
    class SceneGraph : BaseScene
    {
        protected Node _rootNode;
        protected BaseCamera _activeCamera;
        protected YnBasicLight _sceneLight;

        public SceneGraph()
        {
            
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            _rootNode.Up
        }

        public virtual void Draw(GraphicsDevice device)
        {
            _rootNode.Draw(device, _activeCamera, _sceneLight);
        }
    }

    
}
