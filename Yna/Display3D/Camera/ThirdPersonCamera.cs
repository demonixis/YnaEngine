using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public class ThirdPersonCamera : BaseCamera
    {
        private YnBase3D _followedObject;

        public YnBase3D FollowedObject
        {
            get { return _followedObject; }
            set { _followedObject = value; }
        }

        public float Angle
        {
            get { return _reference.Y; }
            set { _reference.Y = value; }
        }

        public float Distance
        {
            get { return _reference.Z; }
            set { _reference.Z = value; }
        }

        public float AngleArround
        {
            get { return _reference.X; }
            set { _reference.X = value; }
        }

        #region Constructors

        public ThirdPersonCamera()
        {
            _reference = new Vector3(0, 25, -25);

            SetupCamera();
        }

        public ThirdPersonCamera(float topAngle, float distanceBetweenObject, float angleArroundObject)
        {
            _reference = new Vector3(angleArroundObject, topAngle, distanceBetweenObject);

            SetupCamera();
        }

        #endregion

        public override void SetupCamera(Vector3 position, Vector3 target, float nearClip, float farClip)
        {
            _position = position;
            _target = target;

            _yaw = 0.0f;
            _pitch = 0.0f;

            _nearClip = nearClip;
            _farClip = farClip;

            _view = Matrix.CreateLookAt(_position, _target, Vector3.Up);

            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);

            _world = Matrix.Identity;
        }

        public override void SetupCamera()
        {
            SetupCamera(new Vector3(0.0f, 0.0f, 5.0f), Vector3.Zero, 1.0f, 3500.0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Matrix matRotation = Matrix.CreateRotationY(_yaw) * Matrix.CreateRotationY(_followedObject.Rotation.Y);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotation);

            Vector3 position = transformedReference + _followedObject.Position;

            _view = Matrix.CreateLookAt(position, _followedObject.Position, Vector3.Up);

            _projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);

            _world = Matrix.Identity;
        }
    }
}
