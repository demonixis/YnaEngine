using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Framework.Display.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display
{
    /// <summary>
    /// The scene of a state
    /// </summary>
    public class YnScene : YnBase
    {
        private List<YnTimer> _timers;
        private List<YnBase> _baseObjects;
        private YnGroup _sceneObjects;
        private YnGui _guiManager;

        public YnScene()
        {
            _timers = new List<YnTimer>();
            _baseObjects = new List<YnBase>();
            _sceneObjects = new YnGroup();
            _guiManager = new YnGui();
        }

        public override void Update(GameTime gameTime)
        {
            int timerCount = _timers.Count;
            int baseCount = _baseObjects.Count;

            if (timerCount > 0)
            {
                foreach (YnTimer timer in _timers)
                    timer.Update(gameTime);
            }

            if (baseCount > 0)
            {
                foreach (YnBase baseObject in _baseObjects)
                    baseObject.Update(gameTime);
            }

            _sceneObjects.Update(gameTime);
            _guiManager.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _sceneObjects.Draw(gameTime, spriteBatch);
            _guiManager.Draw(gameTime, spriteBatch);
        }
    }
}
