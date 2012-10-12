using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class Model3D : YnBase3D
    {
        private BaseCamera _camera;
        private Model _model;
        private string _modelName;
        private Texture2D _texture;
        private BasicEffect _basicEffect;

        private float _width;
        private float _height;
        private float _depth;

        /// <summary>
        /// Get the width of the model
        /// </summary>
        public float Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Get the height of the model
        /// </summary>
        public float Height
        {
            get { return _height; }
        }

        /// <summary>
        /// Get the depth of the model
        /// </summary>
        public float Depth
        {
            get { return _depth; }
        }

        /// <summary>
        /// Get or Set the camera used for this model
        /// </summary>
        public BaseCamera Camera
        {
            get { return _camera; }
            set { _camera = value; }
        }

        public Model3D(string modelName, Vector3 position)
            : base()
        {
            _position = position;
            _modelName = modelName;

            _width = 0;
            _height = 0;
            _depth = 0;
        }

        public void LoadContent()
        {
            _basicEffect = new BasicEffect(YnG.GraphicsDevice);
            _model = YnG.Content.Load<Model>(_modelName);

            BoundingBox boundingBox = GetBoundingBox();

            _width = boundingBox.Max.X - boundingBox.Min.X;
            _height = boundingBox.Max.Y - boundingBox.Min.Y;
            _depth = boundingBox.Max.Z - boundingBox.Min.Z;
        }

        public void SetupShader()
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public virtual BoundingBox GetBoundingBox()
        {
            BoundingBox boundingBox = new BoundingBox();

            foreach (ModelMesh mesh in _model.Meshes)
            {
                float radius = mesh.BoundingSphere.Radius;

                boundingBox.Min.X = boundingBox.Min.X < Position.X ? boundingBox.Min.X : Position.X;
                boundingBox.Min.Y = boundingBox.Min.Y < Position.Y ? boundingBox.Min.Y : Position.Y;
                boundingBox.Min.Z = boundingBox.Min.Z < Position.Z ? boundingBox.Min.Z : Position.Z;

                boundingBox.Max.X = boundingBox.Max.X > Position.X + radius ? boundingBox.Max.X : Position.X + radius;
                boundingBox.Max.Y = boundingBox.Max.X > Position.Y + radius ? boundingBox.Max.Y : Position.Y + radius;
                boundingBox.Max.Z = boundingBox.Max.X > Position.Z + radius ? boundingBox.Max.Z : Position.Z + radius;
            }

            return boundingBox;
        }

        public void Draw(GraphicsDevice device)
        {
            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World =
                         transforms[mesh.ParentBone.Index] *
                        _camera.World * Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationX(Rotation.X) *
                        Matrix.CreateRotationY(Rotation.Y) *
                        Matrix.CreateRotationZ(Rotation.Z) *
                        Matrix.CreateTranslation(Position);


                    effect.View = _camera.View;
                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
