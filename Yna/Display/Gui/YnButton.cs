using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display.Effect;

namespace Yna.Display.Gui
{
    /// <summary>
    /// Base class for buttons
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
            Padding = 10;
            WithBackground = true;
            Pack = false;
        }

        public override void Layout()
        {
            base.Layout();

            if (Pack)
            {
                bounds.Width = GetMaxChildWidth();
                bounds.Height = GetMaxChildHeight();
            }
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

        protected override void DoMouseClick(bool leftClick, bool middleClick, bool rightClick, bool justClicked)
        {
            base.DoMouseClick(leftClick, middleClick, rightClick, justClicked);

            _buttonDown = leftClick && !justClicked;
        }

        /// <summary>
        /// Draws the button borders according to its state
        /// </summary>
        /// <param name="gameTime">Time elasped since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="border">THe border definition to use</param>
        protected override void DrawBorders(GameTime gameTime, SpriteBatch spriteBatch, YnBorder border)
        {
            if (IsHovered)
                base.DrawBorders(gameTime, spriteBatch, Skin.HoveredButtonBorder);
            else
                base.DrawBorders(gameTime, spriteBatch, Skin.PanelBorder);
        }

        /// <summary>
        /// Draws the background according to the button state (normal, hovered or pressed)
        /// </summary>
        /// <param name="gameTime">Time elasped since last draw</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="background">The texture used as background</param>
        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            if (_buttonDown)
                base.DrawBackground(gameTime, spriteBatch, Skin.ClickedButtonBackground);
            else if (IsHovered)
                base.DrawBackground(gameTime, spriteBatch, Skin.HoveredButtonBackground);
            else
                base.DrawBackground(gameTime, spriteBatch, Skin.ButtonBackground);
        }
    }
}
