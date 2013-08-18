// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Yna.Engine.Helpers;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Basic checkbox widget.
    /// </summary>
    public class YnCheckbox : YnButton
    {
        #region Attributes

        /// <summary>
        /// Flag set to true when the box is checked.
        /// </summary>
        protected bool _checked;

        #endregion

        #region Properties

        /// <summary>
        /// The current check state.
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public YnCheckbox()
            : base()
        {
            _hasBackground = true;
            _checked = false;
            Width = 20;
            Height = 20;
        }

        /// <summary>
        /// Constructor with a YnWidgetProperties.
        /// </summary>
        /// <param name="properties">The properties</param>
        public YnCheckbox(YnWidgetProperties properties)
        {
            SetProperties(properties);
        }

        #endregion

        /// <summary>
        /// Manage the check flag
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void DoCustomUpdate(GameTime gameTime)
        {
            if (_clicked)
            {
                // Does it really need explainations?
                _checked = !_checked;
            }
        }

        /// <summary>
        /// Draw the checked square.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The skin to use</param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Draw the square only if the box is checked
            if (_checked)
            {
                // Draw a square with padding (Width /5)
                int padding = Width / 5;
                Texture2D fg = GetSkin().BackgroundDefault;
                Rectangle source = new Rectangle(0, 0, fg.Width, fg.Height);
                Rectangle dest = new Rectangle((int)ScreenPosition.X + padding, (int)ScreenPosition.Y + padding, Width - padding * 2, Height - padding * 2);

                spriteBatch.Draw(fg, dest, source, Color.White);
            }

        }

        /// <summary>
        /// Draw the background.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The skin to use</param>
        protected override void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Always draw the clicked background
            Texture2D background = GetSkin().BackgroundClicked;

            Rectangle source = new Rectangle(0, 0, background.Width, background.Height);
            Rectangle dest = new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Width, Height);

            spriteBatch.Draw(background, dest, source, Color.White);
            
        }
    }
}
