using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Yna.Helpers;
using Yna.Display.Event;

namespace Yna.Display.Gui
{
    /// <summary>
    /// Horizontal slider widget
    /// </summary>
    public class YnSlider : YnWidget
    {
        protected int _minValue;
        protected int _maxValue;
        protected int _currentValue;
        private bool _dragging;
        private YnButton _cursor;
        private YnLabel _labelValue;

        #region Properties

        /// <summary>
        /// The minimum possible value
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        
        /// <summary>
        /// The maximum possible value
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }
        
        /// <summary>
        /// The current progress value
        /// </summary>
        public int Value
        {
            get { return _currentValue; }
            set { _currentValue = (int)MathHelper.Clamp(value, _minValue, _maxValue); }
        }
        
#endregion

        public YnSlider()
            : base()
        {
            _minValue = 0;
            _maxValue = 100;
            _dragging = false;
            _cursor = Add(new YnTextButton());
            _cursor.MouseClick += (s, e) => _dragging = true;
            _cursor.MouseReleasedInside += (s, e) => _dragging = false;
            _cursor.MouseReleasedOutside += (s, e) => _dragging = false;
            _labelValue = Add(new YnLabel());
        }

        protected override void DoCustomUpdate(GameTime gameTime)
        {
            base.DoCustomUpdate(gameTime);
            if (_dragging)
            {
                Vector2 delta = YnG.Mouse.Delta;
                // Horizontal move only
                _cursor.Move((int)-delta.X, 0);
                
                // Ensure the cursor is still in his rail
                _cursor.Position = new Vector2(
                    MathHelper.Clamp(_cursor.Position.X, - Height / 2, Width - Height / 2),
                    _cursor.Position.Y
                );

                // Compute the slider new value
                _currentValue = (int)(_cursor.Position.X + _cursor.Width/2) * _maxValue / Width;

                // Update the label value
                _labelValue.Text = _currentValue.ToString() + "/" + _maxValue;
            }
        }

        public override void Layout()
        {
            base.Layout();
            _cursor.Width = Height;
            _cursor.Height = Height;
            _cursor.Position = new Vector2(-_cursor.Width / 2, 0);
        }

        protected override void DrawWidget(GameTime gameTime, SpriteBatch spriteBatch, YnSkin skin)
        {

            // Background
            int bgHeight = Height / 8;
            Rectangle rect = new Rectangle(
                (int) AbsolutePosition.X,
                (int)AbsolutePosition.Y + Height / 2 - bgHeight/2,
                Width,
                bgHeight
            );

            Texture2D tex = GraphicsHelper.CreateTexture(Skin.DefaultTextColor, 1,1);
            spriteBatch.Draw(tex, rect, Color.White);
        }
    }
}
