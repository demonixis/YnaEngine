using System;
using Microsoft.Xna.Framework;
namespace Yna.Display
{
    public class YnCamera
    {
        protected Matrix _view;
        protected Vector2 _position;
        protected Vector2 _rotation;
        protected float _zoom;

        protected Vector2 _centerScreen;

        #region Properties

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        #endregion

        public YnCamera()
        {
            _view = Matrix.Identity;
            _position = Vector2.Zero;
            _rotation = Vector2.Zero;
            _zoom = 1.0f;

            _centerScreen = new Vector2(YnG.Width / 2, YnG.Height / 2);
        }

        public void Update(GameTime gameTime)
        {
            Matrix rotationTransforms = Matrix.CreateRotationX(_rotation.X) * Matrix.CreateRotationY(_rotation.Y);

            Matrix transltationTransforms = 
                Matrix.CreateTranslation(new Vector3(_position - _centerScreen, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(_centerScreen, 0.0f));

            _view = rotationTransforms * transltationTransforms * Matrix.CreateScale(_zoom);
        }

        public Matrix GetMatrix()
        {
            return _view;
        }
    }
}
