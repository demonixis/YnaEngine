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
        protected Dictionary<string, BoundingBox> _boundingBoxes;

        #region Properties

        public Model Model
        {
            get { return _model; }
        }

        public Dictionary<string, BoundingBox> BoundingBoxes
        {
            get { return _boundingBoxes; }
        }

        #endregion

        #region Constructor

        public YnModel(string modelName, Vector3 position)
            : base(position)
        {
            _modelName = modelName;
            _boundingBoxes = new Dictionary<string, BoundingBox>();
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

            _boundingBox = YnModel.CreateBoundingBox(_model);

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

        #region Static methods

        public static BoundingBox CreateBoundingBox(Model model)
        {
            Vector3 modelMin = new Vector3(float.MaxValue);
            Vector3 modelMax = new Vector3(float.MinValue);

            Matrix[] _transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(_transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                Vector3 meshMax = new Vector3(float.MinValue);
                Vector3 meshMin = new Vector3(float.MaxValue);

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                    byte[] vertexData = new byte[stride * meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(meshPart.VertexOffset * stride, vertexData, 0, meshPart.NumVertices, 1);

                    Vector3 vertexPosition = new Vector3();

                    for (int i = 0, l = vertexData.Length; i < l; i += stride)
                    {
                        vertexPosition.X = BitConverter.ToSingle(vertexData, i);
                        vertexPosition.Y = BitConverter.ToSingle(vertexData, i + sizeof(float));
                        vertexPosition.Z = BitConverter.ToSingle(vertexData, i + sizeof(float) * 2);

                        meshMin = Vector3.Min(meshMin, vertexPosition);
                        meshMax = Vector3.Max(meshMax, vertexPosition);
                    }
                }

                meshMin = Vector3.Transform(meshMin, _transforms[mesh.ParentBone.Index]);
                meshMax = Vector3.Transform(meshMax, _transforms[mesh.ParentBone.Index]);

                modelMin = Vector3.Min(modelMin, meshMin);
                modelMax = Vector3.Max(modelMax, meshMax);
            }

            return new BoundingBox(modelMin, modelMax);
        }

        #endregion
    }
}
