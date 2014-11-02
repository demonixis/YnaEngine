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
using Yna.Engine.Graphics.Event;

namespace Yna.Engine.Graphics.Gui.Widgets
{
    /// <summary>
    /// Horizontal slider widget.
    /// </summary>
    public class YnSlider : YnWidget
    {
        #region Attributes

        protected int _minValue;
        protected int _maxValue;
        protected int _currentValue;
        protected bool _dragging;
        protected YnButton _cursor;
        protected YnLabel _labelValue;

        #endregion

        #region Properties

        /// <summary>
        /// The minimum possible value
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set { 
                _minValue = value;
                UpdateCursor();
            }
        }
        
        /// <summary>
        /// The maximum possible value
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set {
                _maxValue = value;
                UpdateCursor();
            
            }
        }
        
        /// <summary>
        /// The current progress value
        /// </summary>
        public int Value
        {
            get { return _currentValue; }
            set { 
                _currentValue = (int)MathHelper.Clamp(value, _minValue, _maxValue);
                UpdateCursor();
            }
        }

        /// <summary>
        /// Show or hide the slider value
        /// </summary>
        public bool ShowValue 
        {
            get { return _labelValue.Visible; }
            set { _labelValue.Visible = value; }
        }

#endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public YnSlider()
            : base()
        {
            _minValue = 0;
            _maxValue = 100;
            _dragging = false;
            _cursor = Add(new YnTextButton());
            _cursor.MouseClick += (s, e) => _dragging = true;
            _cursor.MouseReleased += (s, e) => _dragging = false;
            //_cursor.MouseReleasedInside += (s, e) => _dragging = false;
            //_cursor.MouseReleasedOutside += (s, e) => _dragging = false;
            _labelValue = Add(new YnLabel());
        }

        /// <summary>
        /// Constructor with a YnWidgetProperties.
        /// </summary>
        /// <param name="properties">The properties</param>
        public YnSlider(YnWidgetProperties properties)
            : this()
        {
            SetProperties(properties);
        }

        #endregion

        /// <summary>
        /// Manage slider movement and value changes.
        /// </summary>
        /// <param name="gameTime">The game time</param>
        protected override void DoCustomUpdate(GameTime gameTime)
        {
            base.DoCustomUpdate(gameTime);
            if (_dragging)
            {
                Vector2 delta = YnG.Mouse.Delta;
                // Horizontal move only
                _cursor.Move((int)delta.X, 0);

                // TODO : handle mouse movement overflow to ease the use
                // Ensure the cursor is still in his rail
                _cursor.Move(MathHelper.Clamp(_cursor.Position.X, -Height / 2, Width - Height / 2), _cursor.Position.Y);

                // Compute the slider new value
                int oldValue = _currentValue;
                _currentValue = (int)(_cursor.Position.X + _cursor.Width / 2) * _maxValue / Width;

                // If the value was modified, trigger the change event
                if (oldValue != _currentValue && Changed != null)
                    Changed(this, new ValueChangedEventArgs<int>(_currentValue));

                // Update the label value
                _labelValue.Text = _currentValue.ToString() + "/" + _maxValue;
            }
            else
            {
            }
        }

        /// <summary>
        /// Update the cursor position according to the slider value
        /// </summary>
        private void UpdateCursor()
        {
            _cursor.Move(_currentValue * Width / (float)_maxValue, _cursor.Position.Y);
        }

        /// <summary>
        /// Set up the cursor size.
        /// </summary>
        /// <param name="skin">The skin to use</param>
        protected override void ApplySkin(YnSkin skin)
        {
            _cursor.Width = Height;
            _cursor.Height = Height;
            _cursor.Move(-_cursor.Width / 2, 0);

            UpdateCursor();
        }

        /// <summary>
        /// Draw the background rail. As the rail does not fill the entire widget bounds
        /// it cannot be handled by the DrawBackground method.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="skin"></param>
        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {
            // Background rail rectangle
            int bgHeight = Height / 8;
            Rectangle rect = new Rectangle(
                (int)ScreenPosition.X,
                (int)ScreenPosition.Y + Height / 2 - bgHeight / 2,
                Width,
                bgHeight
            );

            Texture2D tex = GetSkin().BackgroundClicked;
            spriteBatch.Draw(tex, rect, Color.White);
            
        }

        /// <summary>
        /// Triggered when the value is changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<int>> Changed = null;
    }
}
