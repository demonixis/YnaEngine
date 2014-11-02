using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics.Component
{
    public sealed class YnVirtualStick : YnGroup
    {
        private Vector2 _startPosition;
        private Vector2 _stickPosition;
        private Vector2 _sensitivity;
        private Vector2 _delta;
        private Texture2D _bgTexture;
        private Texture2D _fgTexture;
        private bool _mustCenter;
        private bool _enableMouse;
        private bool _stayVisible;
        private bool _canMove;
        private int _fingerTarget;
        private float _deadZone;
        private string[] _textureNames;
        // Cache vars
        private float _tx;
        private float _ty;
        private float _tdx;
        private float _tdy;
        private bool _tTouched;
        private bool _tReleased;

        public bool StayVisible
        {
            get { return _stayVisible; }
            set { _stayVisible = value; }
        }

        public Vector2 Sensitivity
        {
            get { return _sensitivity; }
            set { _sensitivity = value; }
        }

        public bool EnableMouse
        {
            get { return _enableMouse; }
            set { _enableMouse = value; }
        }

        public int FingerTarget
        {
            get { return _fingerTarget; }
            set { _fingerTarget = value; }
        }

        public float DeadZone
        {
            get { return _deadZone; }
            set { _deadZone = value; }
        }

        public Vector2 Delta
        {
            get { return _delta; }
        }

        public YnVirtualStick()
        {
            _fingerTarget = 0;
            _stayVisible = true;
            _canMove = false;
            _deadZone = 0.2f;
            _enableMouse = true;
            _textureNames = new string[0];
            _sensitivity = new Vector2(1);
        }

        public YnVirtualStick(string background, string foreground)
            : this()
        {
            _textureNames = new string[2] { background, foreground };
        }

        public override void LoadContent()
        {
            if (_textureNames.Length == 2)
            {
                _bgTexture = YnG.Content.Load<Texture2D>(_textureNames[0]);
                _fgTexture = YnG.Content.Load<Texture2D>(_textureNames[1]);
            }
            else
            {
                _bgTexture = LoadFromResources("gamepad_bg.png");
                _fgTexture = LoadFromResources("gamepad_fg.png");
            }

            _rectangle.Width = (int)(_bgTexture.Width * _scale.X);
            _rectangle.Height = (int)(_bgTexture.Height * _scale.Y);
           
            ShowAt(_bgTexture.Width + 10, YnG.Height - (_bgTexture.Height * _scale.Y) - 10);
            _canMove = false;
        }

        private Texture2D LoadFromResources(string asset)
        {
#if WINDOWS_STOREAPP
            var assembly = typeof(YnVirtualStick).GetTypeInfo().Assembly;
#else
            var assembly = Assembly.GetExecutingAssembly();
#endif
            var stream = assembly.GetManifestResourceStream("Yna.Engine.Graphics.Component.Resources." + asset);

            return Texture2D.FromStream(YnG.GraphicsDevice, stream);
        }

        public override void Update(GameTime gameTime)
        {
            _tx = YnG.Touch.GetPosition(_fingerTarget).X;
            _ty = YnG.Touch.GetPosition(_fingerTarget).Y;
            _tdx = YnG.Touch.GetDelta(_fingerTarget).X;
            _tdy = YnG.Touch.GetDelta(_fingerTarget).Y;
            _tTouched = YnG.Touch.Pressed(_fingerTarget);
            _tReleased = YnG.Touch.Released(_fingerTarget);

            if (_enableMouse)
            {
                _tx += YnG.Mouse.X;
                _ty += YnG.Mouse.Y;
                _tdx += YnG.Mouse.Delta.X;
                _tdy += YnG.Mouse.Delta.Y;
                _tTouched = _tTouched || YnG.Mouse.Click(MouseButton.Left);
                _tReleased = _tReleased || Mouse.GetState().LeftButton == ButtonState.Released;
            }

            if (!_canMove && _tTouched)
                ShowAt(_tx, _ty);

            else if (_tReleased)
                _canMove = false;

            if (_canMove)
            {
                _stickPosition.X += _tdx;
                _stickPosition.Y += _tdy;

                if (_stickPosition.X < X - _fgTexture.Width * _scale.X)
                    _stickPosition.X = X - _fgTexture.Width * _scale.Y;

                else if (_stickPosition.X > X + _bgTexture.Width * _scale.X)
                    _stickPosition.X = X + _bgTexture.Width * _scale.X;

                if (_stickPosition.Y < Y - _fgTexture.Height * _scale.Y)
                    _stickPosition.Y = Y - _fgTexture.Height * _scale.Y;

                else if (_stickPosition.Y > Y + _bgTexture.Height * _scale.Y)
                    _stickPosition.Y = Y + _bgTexture.Height * _scale.Y;

                _delta.X = round((_stickPosition.X - _startPosition.X) / _bgTexture.Width * _scale.X);
                _delta.Y = round((_stickPosition.Y - _startPosition.Y) / _bgTexture.Height * _scale.Y);

                _delta.X = ((Math.Abs(_delta.X) < _deadZone) ? 0.0f : _delta.X) * _sensitivity.X;
                _delta.Y = ((Math.Abs(_delta.Y) < _deadZone) ? 0.0f : _delta.Y) * _sensitivity.Y;

                _mustCenter = true;
            }
            else if (_mustCenter)
            {
                _stickPosition.X = _startPosition.X;
                _stickPosition.Y = _startPosition.Y;
                _delta.X = 0;
                _delta.Y = 0;
                _mustCenter = false;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_canMove || _stayVisible)
            {
                spriteBatch.Draw(_bgTexture, _position, null, _color * _alpha, _rotation, _origin, _scale, _effects, _layerDepth);
                spriteBatch.Draw(_fgTexture, _stickPosition, null, _color * _alpha, _rotation, _origin, _scale, _effects, _layerDepth);
            }
        }

        public void ShowAt(float x, float y)
        {
            _stickPosition.X = x - _fgTexture.Width / 2 * _scale.X;
            _stickPosition.Y = y - _fgTexture.Height / 2 * _scale.Y;

            _startPosition = new Vector2(_stickPosition.X, _stickPosition.Y);
            _position = new Vector2(x - _bgTexture.Width / 2 * _scale.X, y - _bgTexture.Height / 2 * _scale.Y);
            _mustCenter = false;
            _canMove = true;
            _delta.X = 0.0f;
            _delta.Y = 0.0f;
        }

        private float round(float value)
        {
            return (float)Math.Round(value * 1000.0f) / 1000.0f;
        }
    }
}
