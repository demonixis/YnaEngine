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

            _boundingBox = GetBoundingBox();

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;
        }

        public override BoundingBox GetBoundingBox()
        {
            return GetBoundingBox(true);
        }

        public virtual BoundingBox GetBoundingBox(bool updateBoundingBoxes)
        {
            if (updateBoundingBoxes)
                _boundingBoxes.Clear();

            // Global boundingBox
            BoundingBox boundingBox = new BoundingBox();

            Vector3 min = new Vector3(float.MaxValue);
            Vector3 max = new Vector3(float.MinValue);

            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            UpdateMatrix();

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0, l = vertexData.Length; i < l; i += vertexStride / sizeof(float))
                    {
                        Vector3 position = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);
                        Vector3 transformedPosition = Vector3.Transform(position, transforms[mesh.ParentBone.Index] * World * View);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }

                // Update boudingBox for all mesh in model
                if (updateBoundingBoxes)
                {
                    string tempName = mesh.Name;
                    string name = mesh.Name;

                    int cpt = 0;

                    while (_boundingBoxes.ContainsKey(name))
                    {
                        name = String.Format("{0}_{1}", tempName, cpt.ToString());
                        cpt++;
                    }

                    _boundingBoxes.Add(mesh.Name, new BoundingBox(min, max));
                }
            }

            boundingBox.Min = min;
            boundingBox.Max = max;

            return boundingBox;
        }

        private void UpdateMatrix()
        {
            World =
                Matrix.CreateScale(Scale) *
                Matrix.CreateRotationX(Rotation.X) *
                Matrix.CreateRotationY(Rotation.Y) *
                Matrix.CreateRotationZ(Rotation.Z) *
                Matrix.CreateTranslation(Position) *
                _camera.World;

            View = _camera.View;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateMatrix();

            _boundingBox = GetBoundingBox();     
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

                    effect.World = transforms[mesh.ParentBone.Index] * World;

                    effect.View = View;

                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
