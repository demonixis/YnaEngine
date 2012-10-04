using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D.Camera
{
    public enum MoveType
    {
        Up = 0, Down, Left, Right
    }
	
    public class FirstPersonCamera
    {
        private Game game;
        private Matrix view;
        private Matrix projection;
        private Matrix world;
		
		// Rotation sur Y et X
        protected float _yaw;
		
		// Paramètrage de la caméra
		protected float _nearClip;
		protected float _farClip;
		
		// Position, cible, placement
		protected Vector3 _reference;
		protected Vector3 _position;
		protected Vector3 _target;
		
		#region Propriétés
		public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }
		
		public float Yaw
		{
			get { return _yaw; }
			set { _yaw = value; }
		}
		
		public float Near
		{
			get { return _nearClip; }
			set { _nearClip = value; }
		}
		
		public float Far
		{
			get { return _farClip; }
			set { _farClip = value; }
		}
		
		public Vector3 Reference
		{
			get { return _reference; }
			set { _reference = value; }
		}
		
        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

		public float AspectRatio
		{
			get { return game.GraphicsDevice.Viewport.Width / game.GraphicsDevice.Viewport.Height; }	
		}
		
		public float FieldOfView
		{
			get { return MathHelper.PiOver4; }	
		}
		
		#endregion

        public FirstPersonCamera()
        {
            
        }

        public void Initialize()
        {
            _reference = new Vector3(0.0f, 0.0f, 10.0f);
            _position = new Vector3(0.0f, 0.0f, 5.0f);
            _target = Vector3.Zero;

            _yaw = 0.0f;

            _nearClip = 1.0f;
            _farClip = 3500.0f;

            CreateCamera();
        }

        protected void CreateCamera()
        {
            view = Matrix.CreateLookAt(_position, _target, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
            world = Matrix.Identity;
        }
		
		public void Translate(Vector3 move)
		{
			Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
            Vector3 v = Vector3.Transform(move, forwardMovement);
            _position.X += v.X;
            _position.Z += v.Z;
		}

        public void Translate(float x, float y, float z)
        {
            Vector3 move = new Vector3(x, y, z);
            Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
            Vector3 v = Vector3.Transform(move, forwardMovement);
            _position.X += v.X;
            _position.Y += v.Y;
            _position.Z += v.Z;
        }
		
		public void RotateY(float angle)
		{
            _yaw += MathHelper.ToRadians(angle);

			// Eviter les depassements
			if ((_yaw >= MathHelper.Pi * 2) || (_yaw <= -MathHelper.Pi * 2))
				_yaw = 0.0f;
		}

        public void Update(GameTime gameTime)
        {
            Matrix matRotationY = Matrix.CreateRotationY(_yaw);

            Vector3 transformedReference = Vector3.Transform(_reference, matRotationY);

            _target = _position + transformedReference;

            view = Matrix.CreateLookAt(_position, _target, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
        }
    }
}