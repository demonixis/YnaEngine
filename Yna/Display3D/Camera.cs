﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D
{
	public enum CameraType
	{
		Fixed = 0, FirstPerson = 1, ThirdPerson = 2
	}
	
    public class Camera
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
		protected CameraType _cameraType;
		
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
		
		public CameraType CameraType
		{
			get { return _cameraType; }
			set { _cameraType = value; }
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

        public Camera(Game game)
        {
            this.game = game;

			_cameraType = CameraType.FirstPerson;
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
		
		public void RotateY(float angle)
		{
            _yaw += MathHelper.ToRadians(angle);

			// Eviter les depassements
			if ((_yaw >= MathHelper.Pi * 2) || (_yaw <= -MathHelper.Pi * 2))
				_yaw = 0.0f;
		}
		
		public void UpdateFirstPerson(GameTime gameTime)
		{
			Matrix matRotationY = Matrix.CreateRotationY(_yaw);

			Vector3 transformedReference = Vector3.Transform(_reference, matRotationY);
			
			_target = _position + transformedReference;
			
			view = Matrix.CreateLookAt (_position, _target, Vector3.Up);
			projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
		}
		
		public void UpdateFixed(GameTime gameTime)
		{
			
			view = Matrix.CreateLookAt (_position, _reference, Vector3.Up);
			
			projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, Near, Far);
		}
		

        public void Update(GameTime gameTime)
        {
            if (_cameraType == CameraType.FirstPerson)
				UpdateFirstPerson(gameTime);
			else if (_cameraType == CameraType.Fixed)
				UpdateFixed(gameTime);
        }
    }
}