using System;
using Microsoft.Xna.Framework;
namespace Yna.Framework.Display
{
    public class YnCamera
    {
        protected Matrix _view;
        protected int _x;
        protected int _y;
        protected float _rotation;
        protected float _zoom;
        protected Vector2 _centerScreen;

        #region Properties

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public float Rotation
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
            _x = 0;
            _y = 0;
            _rotation = 0.0f;
            _zoom = 1.0f;

            _centerScreen = new Vector2(YnG.Width / 2, YnG.Height / 2);
        }


        public Matrix GetTransformMatrix()
        {
            Matrix translateToOrigin = Matrix.CreateTranslation(_x + (-YnG.Width / 2), _y + (-YnG.Height / 2), 0);
            Matrix rotation = Matrix.CreateRotationZ(MathHelper.ToRadians(_rotation));
            Matrix zoom = Matrix.CreateScale(_zoom);
            Matrix translateBackToPosition = Matrix.CreateTranslation(_x + (YnG.Width / 2), _y + (YnG.Height / 2), 0);
            Matrix composition = zoom * translateToOrigin * rotation * translateBackToPosition;

            return composition;
        }
    }
}
