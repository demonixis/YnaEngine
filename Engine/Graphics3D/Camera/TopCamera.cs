using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Camera
{
    /// <summary>
    /// This camera shows an view on Y axis
    /// </summary>
    public class TopCamera : BaseCamera
    {
        protected int _height;
        protected YnEntity3D _followedObject;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public new Vector3 Target
        {
            get
            {
                return _followedObject != null ? _followedObject.Position : _target;
            }
        }

        public TopCamera(Vector3 target, int height)
            : base()
        {
            _target = target;
            _height = height;
            _position = new Vector3(target.X, height, target.Z);
        }

        public TopCamera(YnEntity3D followedModel, int height)
            : this(followedModel.Position, height)
        {
            _followedObject = followedModel;
        }

        public override void SetupCamera()
        {
            //SetupCamera(new Vector3(0, _height, 0), Vector3.Zero, 1.0f, 3500.0f);
            _yaw = 0.0f;
            _pitch = 0.0f;
            _roll = 0.0f;
            _nearClip = 1f;
            _farClip = 3500f;

            _view = Matrix.CreateLookAt(_position, Target, _vectorUp);

            _world = Matrix.Identity;

            UpdateProjection();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //_reference = new Vector3(_followedObject.X, _height, _followedObject.Z);
            if (_followedObject != null)
            {
                _position = new Vector3(Target.X, _height, Target.Z - 0.0001f); // FIXME : Why vertical is impossible?
            }

            _view = Matrix.CreateLookAt(_position, Target, _vectorUp);

            _world = Matrix.Identity;
        }
    }
}
