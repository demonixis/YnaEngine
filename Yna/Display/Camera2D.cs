using System;
using Microsoft.Xna.Framework;

namespace Yna.Display
{
    public struct ScrollFactor
    {
        public bool X;
        public bool Y;

        public ScrollFactor(bool x, bool y)
        {
            X = x;
            Y = y;
        }
    }

	public class Camera2D
	{
		private Rectangle _globalCamera;
		private Rectangle _screenCamera;
		private YnSprite _sprite;
        private Vector2 _spritePosition;
        private bool _follow;

        public int X
        {
            get { return _screenCamera.X; }
            set { _screenCamera = new Rectangle(value, Y, Width, Height); }
        }

        public int Y
        {
            get { return _screenCamera.Y; }
            set { _screenCamera = new Rectangle(X, value, Width, Height); }
        }

        public int Width
        {
            get { return _screenCamera.Width; }
            set { _screenCamera = new Rectangle(X, Y, value, Height); }
        }

        public int Height
        {
            get { return _screenCamera.Height; }
            set { _screenCamera = new Rectangle(X, Y, Width, value); }
        }
		
		public Vector2 ScrollMax
		{
			get 
            {
                return new Vector2(_globalCamera.Width - _screenCamera.Width, _globalCamera.Height - _screenCamera.Height); 
            }	
		}
		
		public Vector2 ScrollMin	
		{
			get { return new Vector2(_screenCamera.Width / 2, _screenCamera.Height / 2); }	
		}

        public bool Follow
        {
            get { return _follow; }
        }

        public Rectangle Bounds
        {
            get { return _globalCamera; }
            set { _globalCamera = value; }
        }

        public Vector2 SpritePosition
        {
            get { return _spritePosition; }
            set { _spritePosition = value; }
        }
		
		public Camera2D ()
		{
			_screenCamera = new Rectangle(0, 0, YnG.Width, YnG.Height);
			_globalCamera = new Rectangle(_screenCamera.X, _screenCamera.Y, _screenCamera.Width, _screenCamera.Height);
			_sprite = null;
            _spritePosition = Vector2.Zero;
			_follow = false;
		}
		
		public void follow(YnSprite sprite)
		{
			_sprite = sprite;
			_follow = true;
		}
	}
}

