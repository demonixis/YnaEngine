using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Display3D
{
    public abstract class Mesh
    {
        protected Game game;
        protected Camera camera;

        protected Vector3 position;
        protected Vector3 rotation;
        protected Vector3 scale;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float RotationX
        {
            get { return rotation.X; }
            set { rotation = new Vector3(value, rotation.Y, rotation.Z); }
        }

        public float RotationY
        {
            get { return rotation.Y; }
            set { rotation = new Vector3(rotation.X, value, rotation.Z); }
        }

        public float RotationZ
        {
            get { return rotation.Z; }
            set { rotation = new Vector3(rotation.X, rotation.Y, value); }
        }

        public float PositionX
        {
            get { return position.X; }
            set { position = new Vector3(value, position.Y, position.Z); }
        }

        public float PositionY
        {
            get { return position.Y; }
            set { position = new Vector3(position.X, value, position.Z); }
        }

        public float PositionZ
        {
            get { return position.Z; }
            set { position = new Vector3(position.X, position.Y, value); }
        }

        public Camera Camera
        {
            get { return camera; }
            set { camera = value; }
        }

        public Game Game
        {
            get { return game; }
        }

        public Mesh(Game game, Camera camera)
        {
            this.game = game;
            this.camera = camera;

            position = Vector3.Zero;
            rotation = Vector3.Zero;
            scale = Vector3.One;
        }

        public abstract void Initialize();

        public abstract void Draw(GameTime gameTime);
    }
}
