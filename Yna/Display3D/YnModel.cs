using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public class YnModel : YnObject3D
    {
        protected Model _model;
        protected string _modelName;

        #region Properties

        /// <summary>
        /// Get the Model instance
        /// </summary>
        public Model Model
        {
            get { return _model; }
        }

        /// <summary>
        /// Get all meshes off the model
        /// </summary>
        public ModelMesh [] Meshes
        {
            get
            {
                ModelMesh[] meshes = new ModelMesh[_model.Meshes.Count];
                _model.Meshes.CopyTo(meshes, 0);
                return meshes;
            }
        }

        /// <summary>
        /// Get all bones off the model
        /// </summary>
        public ModelBone[] Bones
        {
            get
            {
                ModelBone[] bones = new ModelBone[_model.Bones.Count];
                _model.Bones.CopyTo(bones, 0);
                return bones;
            }
        }

        #endregion

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

        #region GameState Pattern

        public override void LoadContent()
        {
            base.LoadContent();

            _model = YnG.Content.Load<Model>(_modelName);

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;
        }

        public override void Draw(GraphicsDevice device)
        {
            World = Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationX(Rotation.X) *
                    Matrix.CreateRotationY(Rotation.Y) *
                    Matrix.CreateRotationZ(Rotation.Z) *
                    Matrix.CreateTranslation(Position); 

            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * World;

                    effect.View = _camera.View;

                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}
