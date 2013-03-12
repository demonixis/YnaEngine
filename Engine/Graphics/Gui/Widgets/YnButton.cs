using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Animation;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Base class for all kinds of buttons
    /// </summary>
    public abstract class YnButton : YnPanel
    {
        /// <summary>
        /// Useful flag to know if the button is currently pressed
        /// </summary>
        protected bool _buttonDown;

        public YnButton()
            : base()
        {
            _padding = 10;
            _hasBackground = true;
            _animated = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">Time elasped since last update</param>
        protected override void DoCustomUpdate(GameTime gameTime)
        {
            // Reset the button state to be handled properly
            _buttonDown = false;
        }

        /*
        protected override void DoMouseClick(bool leftClick, bool middleClick, bool rightClick, bool justClicked)
        {
            base.DoMouseClick(leftClick, middleClick, rightClick, justClicked);

            _buttonDown = leftClick && !justClicked;
        }
*/
    }
}
