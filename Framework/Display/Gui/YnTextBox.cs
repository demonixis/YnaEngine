using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display.Gui
{
    /// <summary>
    /// Text box widget. Provies wrapping and elipsis processing
    /// </summary>
    public class YnTextBox : YnPanel
    {
        #region Attributes

        protected  string _text;
        protected bool _wrapLines;
        protected bool _wordWrap;

        #endregion

        #region Properties

        /// <summary>
        /// The text diplayed in the textbox
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Layout();
            }
        }

        /// <summary>
        /// Set to true to wrap the text. If set to false, the text will be truncated
        /// with "..." 
        /// </summary>
        public bool Wrap
        {
            get { return _wrapLines; }
            set { _wrapLines = value; }
        }

        /// <summary>
        /// Set to true to wrap words instead of characters (default to true)
        /// </summary>
        public bool WordWrap
        {
            get { return _wordWrap; }
            set { _wordWrap = value; }
        }

        #endregion

        public YnTextBox(string text, int width, bool wrapLines)
            : base()
        {
            Width = width;
            _text = text;
            _wrapLines = wrapLines;
            _wordWrap = true;
            _orientation = YnOrientation.Vertical;
        }

        public override void Layout()
        {
            _children.Clear();

            if(_wrapLines)
            {
                // Text must be cropped into lines
                if (_wordWrap)
               {
                    // Wrap based on words
                    WrapWords();
               }
               else
               {
                    // Wrap based on characters
                    WrapChars();
               }
            }
            else
            {
                // Text elipsis
                CharacterElipsis();
            }

            // Set the skin to newly created labels
            InitSkin();

            base.Layout();
        }

        #region Wrapping methods
        
        private void WrapChars()
        {
            SpriteFont font = _skin.Font;
            string buffer = "";
            for (int i = 0; i < _text.Length; i++)
            {
                if (font.MeasureString(buffer + _text[i]).X > Width)
                {
                    // Too long : wrap line
                    Add(new YnLabel(buffer));
                    buffer = "";
                    buffer += _text[i];
                }
                else
                {
                    // Length is ok
                    buffer += _text[i];
                }
            }

            Add(new YnLabel(buffer));
        }

        private void WrapWords()
        {
            SpriteFont font = _skin.Font;
            string[] words = _text.Split(' ');
            string buffer = "";
            for (int i = 0; i < words.Length; i++ )
            {
                if (font.MeasureString(buffer + words[i]).X > Width)
                {
                    // Too long : wrap line
                    Add(new YnLabel(buffer));
                    buffer = words[i] + " ";
                }
                else
                {
                    // Length is ok
                    buffer += words[i] + " ";
                }
            }
            Add(new YnLabel(buffer));
        }

        private void CharacterElipsis()
        {
            SpriteFont font = _skin.Font;
            string buffer = "";
            int i = 0;
            while (font.MeasureString(buffer + "...").X <= Width && i < _text.Length)
            {
                buffer += _text[i];
                i++;
            }

            Add(new YnLabel(buffer + "..."));
        }

        #endregion

    }
}
