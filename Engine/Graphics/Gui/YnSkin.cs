// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Gui
{
    /// <summary>
    /// Global skin definition. Contains all possible states rendering details. Rendering modes are "default", 
    /// "hovered" and "clicked". Each group defines the same properties for a different state. Default definitions are
    /// mandatory, the two others may be skipped. If done so, retreiving a hover font will return the default font (for 
    /// example). 
    /// </summary>
    public class YnSkin
    {
        #region Attributes

        // Default state
        protected SpriteFont _fontDefault;
        protected string _fontNameDefault;
        protected Color _textColorDefault;
        protected YnBorder _borderDefault;
        protected Texture2D _backgroundDefault;

        // Hover state
        protected SpriteFont _fontHover;
        protected string _fontNameHover;
        protected Color _textColorHover;
        protected YnBorder _borderHover;
        protected Texture2D _backgroundHover;

        // Clicked state
        protected SpriteFont _fontClicked;
        protected string _fontNameClicked;
        protected Color _textColorClicked;
        protected YnBorder _borderClicked;
        protected Texture2D _backgroundClicked;

        #endregion

        #region Default state

        /// <summary>
        /// The font for default mode. Must be filled.
        /// </summary>
        public SpriteFont FontDefault
        {
            get { return _fontDefault; }
            set { _fontDefault = value; }
        }

        /// <summary>
        /// The font name for default mode. Must be filled.
        /// </summary>
        public string FontNameDefault
        {
            get { return _fontNameDefault; }
            set { _fontNameDefault = value; }
        }

        /// <summary>
        /// The text color for default mode. Must be filled.
        /// </summary>
        public Color TextColorDefault
        {
            get { return _textColorDefault; }
            set { _textColorDefault = value; }
        }

        /// <summary>
        /// The border for default mode. Can be left blank.
        /// </summary>
        public YnBorder BorderDefault
        {
            get { return _borderDefault; }
            set { _borderDefault = value; }
        }

        /// <summary>
        /// The background for default mode. Can be left blank.
        /// </summary>
        public Texture2D BackgroundDefault
        {
            get { return _backgroundDefault; }
            set { _backgroundDefault = value; }
        }

        #endregion

        #region Hover state

        /// <summary>
        /// The font for Hover mode. Can be left blank.
        /// </summary>
        public SpriteFont FontHover
        {
            get { return _fontHover != null ? _fontHover : _fontDefault; }
            set { _fontHover = value; }
        }

        /// <summary>
        /// The font name for Hover mode. Can be left blank.
        /// </summary>
        public string FontNameHover
        {
            get { return _fontNameHover != null ? _fontNameHover : _fontNameDefault; }
            set { _fontNameHover = value; }
        }

        /// <summary>
        /// The text color for Hover mode. Can be left blank.
        /// </summary>
        public Color TextColorHover
        {
            get { return _textColorHover != null ? _textColorHover : _textColorDefault; }
            set { _textColorHover = value; }
        }

        /// <summary>
        /// The border for Hover mode. Can be left blank.
        /// </summary>
        public YnBorder BorderHover
        {
            get { return _borderHover != null ? _borderHover : _borderDefault; }
            set { _borderHover = value; }
        }

        /// <summary>
        /// The background for Hover mode. Can be left blank.
        /// </summary>
        public Texture2D BackgroundHover
        {
            get { return _backgroundHover != null ? _backgroundHover : _backgroundDefault; }
            set { _backgroundHover = value; }
        }

        #endregion

        #region Clicked state

        /// <summary>
        /// The font for Clicked mode. Can be left blank.
        /// </summary>
        public SpriteFont FontClicked
        {
            get { return _fontClicked != null ? _fontClicked : _fontDefault; }
            set { _fontClicked = value; }
        }

        /// <summary>
        /// The font name for Clicked mode. Can be left blank.
        /// </summary>
        public string FontNameClicked
        {
            get { return _fontNameClicked != null ? _fontNameClicked : _fontNameDefault; }
            set { _fontNameClicked = value; }
        }

        /// <summary>
        /// The text color for Clicked mode. Can be left blank.
        /// </summary>
        public Color TextColorClicked
        {
            get { return _textColorClicked != null ? _textColorClicked : _textColorDefault; }
            set { _textColorClicked = value; }
        }

        /// <summary>
        /// The border for Clicked mode. Can be left blank.
        /// </summary>
        public YnBorder BorderClicked
        {
            get { return _borderClicked != null ? _borderClicked : _borderDefault; }
            set { _borderClicked = value; }
        }

        /// <summary>
        /// The background for Clicked mode. Can be left blank.
        /// </summary>
        public Texture2D BackgroundClicked
        {
            get { return _backgroundClicked != null ? _backgroundClicked : _backgroundDefault; }
            set { _backgroundClicked = value; }
        }

        #endregion
    }
}
