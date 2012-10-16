using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class YnModel : YnObject3D
    {
        protected Model _model;
        protected string _modelName;


        #region Constructor

        public YnModel(string modelName, Vector3 position)
            : base(position)
        {
            _modelName = modelName;
        }

        public YnModel(string modelName)
            : this(modelName, new Vector3(0.0f, 0.0f, 0.0f))
        {

        }

        #endregion

        public override void LoadContent()
        {
            base.LoadContent();

            _model = YnG.Content.Load<Model>(_modelName);

            _boundingBox = GetBoundingBox();
        }

        public override BoundingBox GetBoundingBox()
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

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            return boundingBox;
        }

        public override void Draw(GraphicsDevice device)
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
                        Matrix.CreateScale(Scale) *
                        Matrix.CreateRotationX(Rotation.X) *
                        Matrix.CreateRotationY(Rotation.Y) *
                        Matrix.CreateRotationZ(Rotation.Z) *
                        Matrix.CreateTranslation(Position) *
                        _camera.World;

                    effect.View = _camera.View;
                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
