using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Framework.Display.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display
{
    public class YnScene : YnBase
    {
        // Basic object (juste an Update() method)
        private List<YnBase> _safeBaseObjects;
        private List<YnBase> _baseObjectsToRemove;

        // Graphic objects (Update() and Draw() methods)
        private List<YnObject> _safeSceneObjects;
        private List<YnObject> _sceneObjectsToRemove;

        // Screen gui
        private YnGui _gui;

        public override void Update(GameTime gameTime)
        {
        
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
