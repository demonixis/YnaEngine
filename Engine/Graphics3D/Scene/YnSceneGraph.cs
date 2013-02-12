using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics;
using Yna.Engine.Graphics.Scene;
using Yna.Engine.Graphics3D.Lighting;

namespace Yna.Engine.Graphics3D.Scene
{
    class YnSceneGraph : BaseScene
    {
        protected YnBasicLight _light;
        private List<YnNode> _sceneNodes;

        public YnSceneGraph()
        {
            _light = new YnBasicLight();
            _sceneNodes = new List<YnNode>(); 
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent()
        {
            
        }

        public override void Clear()
        {
            _sceneNodes.Clear();
        }
    }

    
}
