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
    /// Defines a text alignment in a YnLabel.
    /// </summary>
    public enum YnTextAlign { None, TopLeft, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight }

    /// <summary>
    /// This widget displays text. The text can be aligned in the box defined by it's width / height
    /// but can also be used as a simple text renderer with no alignment.
    /// </summary>
    public class YnLabel : YnWidget
    {
        #region Attributes

        protected YnTextAlign _textAlign;
        protected string _text;
        protected int _textWidth;
        protected int _textHeight;
        protected Vector2 _textPosition;
        protected Color _textColor;
        protected bool _useCustomColor;
        protected SpriteFont _customFont;

        #endregion

        #region Properties
        
        /// <summary>
        /// The displayed text.
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The text custom color. To use this custom color, the _useCustomColor flag 
        /// must be set to true. See UseCustomColor.
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                _useCustomColor = true;
            }
        }

        /// <summary>
        /// Set to true to use the custom color defined in TextColor.
        /// </summary>
        public bool UseCustomColor
        {
            get { return _useCustomColor; }
            set { _useCustomColor = value; }
        }

        /// <summary>
        /// A custom font may be used to display this label's text. This font will be 
        /// used instead of the one defined in the widget's skin.
        /// </summary>
        /// 
        public SpriteFont CustomFont
        {
            get { return _customFont; }
            set { _customFont = value; }
        }

        /// <summary>
        /// Set up the text alignment. Be sure to define Width and Height or strange
        /// things could happen!
        /// </summary>
        public YnTextAlign TextAlign
        {
            get { return _textAlign; }
            set
            {
                _textAlign = value;
                ComputeTextAlignment();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public YnLabel() 
            : base() 
        {
            _text = "";
            _useCustomColor = false;

            // No text alignment by default
            _textAlign = YnTextAlign.None;
            _textPosition = Vector2.Zero;
            _textWidth = 0;
            _textHeight = 0;
        }

        /// <summary>
        /// Create a label with the given text.
        /// </summary>
        /// <param name="text">The label text</param>
        public YnLabel(string text)
            : this()
        {
            _text = text;
        }

        /// <summary>
        /// Constructor with a YnWidgetProperties.
        /// </summary>
        /// <param name="properties">The widget properties</param>
        public YnLabel(YnWidgetProperties properties)
            : this()
        {
            SetProperties(properties);
        }

        #endregion

        /// <summary>
        /// See documentation in YnWidget.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The sprite batch</param>
        /// <param name="skin">The skin used to render the widget</param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // The label does not react to user input so we simply get the default text color
            Color color = (_useCustomColor) ? _textColor : skin.TextColorDefault;
            SpriteFont font = (_customFont == null) ? skin.FontDefault : _customFont;

            // Draw the text at the aligned position
            spriteBatch.DrawString(font, _text, ScreenPosition + _textPosition, color, _rotation, Vector2.Zero, _scale, SpriteEffects.None, 1.0f);
            
        }

        /// <summary>
        /// Computes the text size and relative coordinates according to the alignment defined.
        /// </summary>
        /// <param name="skin">The skin</param>
        protected override void ApplySkin(YnSkin skin)
        {
            // If a custom font is currently in use, take it to measure the text
            SpriteFont font = (_useCustomColor && _customFont != null) ? _customFont : skin.FontDefault;

            // Measure the text to define widget's width & height
            Vector2 size = font.MeasureString(Text);
            _textWidth = (int)( size.X * _scale.X);
            _textHeight = (int)( size.Y * _scale.Y);

            // 
            if (_textAlign != YnTextAlign.None)
            {
                // An alignment is defined, recompute coordinates
                ComputeTextAlignment();
            }
            else
            {
                // No alignment, the widget size is initialized with the text size
                Width = _textWidth;
                Height = _textHeight;
            }
        }

        /// <summary>
        /// Compute the text relative position in the widget.
        /// </summary>
        public void ComputeTextAlignment()
        {
            int width = _textWidth;
            int height = _textHeight;

            Vector2 pos = Vector2.Zero;
            switch (_textAlign)
            {
                case YnTextAlign.TopLeft:
                    pos = Vector2.Zero;
                    break;
                case YnTextAlign.Top:
                    pos = new Vector2(Width / 2 - width / 2, 0);
                    break;
                case YnTextAlign.TopRight:
                    pos = new Vector2(Width - width, 0);
                    break;
                case YnTextAlign.Left:
                    pos = new Vector2(0, Height / 2 - height / 2);
                    break;
                case YnTextAlign.Center:
                    pos = new Vector2(Width / 2 - width / 2, Height / 2 - height / 2);
                    break;
                case YnTextAlign.Right:
                    pos = new Vector2(Width - width, Height / 2 - height / 2);
                    break;
                case YnTextAlign.BottomLeft:
                    pos = new Vector2(0, Height - height);
                    break;
                case YnTextAlign.Bottom:
                    pos = new Vector2(Width / 2 - width / 2, Height - height);
                    break;
                case YnTextAlign.BottomRight:
                    pos = new Vector2(Width - width, Height - height);
                    break;
            }
            _textPosition = pos;
        }
    }
}
