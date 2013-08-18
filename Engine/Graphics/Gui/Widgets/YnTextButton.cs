// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Text button widget. The text can be aligned inside the button.
    /// See YnLabel for more informations about text alignment.
    /// </summary>
    public class YnTextButton : YnButton
    {
        #region Attributes
        
        private YnLabel _label;

        #endregion

        #region Properties

        /// <summary>
        /// The text alignment inside the button.
        /// </summary>
        public YnTextAlign TextAlign
        {
            get { return _label.TextAlign; }
            set
            {
                _label.TextAlign = value;
                // Recompute text alignment
                _label.ComputeTextAlignment();
            }
        }

        /// <summary>
        /// The text displayed in the button.
        /// </summary>
        public string Text
        {
            get { return _label.Text; }
            set { _label.Text = value;}
        }

        /// <summary>
        /// Overrides the skin font and use this one instead.
        /// </summary>
        public SpriteFont CustomFont
        {
            set { _label.CustomFont = value; }
        }

        /// <summary>
        /// The button width.
        /// </summary>
        public new int Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                _label.Width = value;
            }
        }

        /// <summary>
        /// The button height.
        /// </summary>
        public new int Height
        {
            get { return base.Height; }
            set
            {
                base.Height = value;
                _label.Height = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public YnTextButton()
            : base()
        {
            _label = new YnLabel();
            _label.TextAlign = YnTextAlign.Center; // Default text alignment
            Add(_label);

            // Default width / height
            Width = 150;
            Height = 40;
        }

        /// <summary>
        /// Constructor with the button text.
        /// </summary>
        /// <param name="text">the button text</param>
        public YnTextButton(string text)
             : this()
        {
            _label.Text = text;
        }

        /// <summary>
        /// Constructor with a YnWidgetProperties.
        /// </summary>
        /// <param name="properties">the widget's properties</param>
        public YnTextButton(YnWidgetProperties properties)
            : this()
        {
            SetProperties(properties);

            // Button's width / height must be set on the label too if defined
            if(properties.Width != null)
            {
                _label.Width = (int) properties.Width;
            }

            if (properties.Height != null)
            {
                _label.Height = (int)properties.Height;
            }
        }

        #endregion

        /// <summary>
        /// Manage the text color when the button is clicked.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void DoCustomUpdate(GameTime gameTime)
        {
            if (Clicked)
            {
                // The label's custom text color is used to render it in the clicked state
                _label.TextColor = YnGui.GetSkin(_skinName).TextColorClicked;
                _label.UseCustomColor = true;
            }
            else
            {
                _label.UseCustomColor = false;
            }

        }

        /// <summary>
        /// Apply the skin to the inner label.
        /// </summary>
        /// <param name="skin">The skin</param>
        protected override void ApplySkin(YnSkin skin)
        {
            _label.SkinName = _skinName;
            _label.ApplySkin();
        }

    }
}
