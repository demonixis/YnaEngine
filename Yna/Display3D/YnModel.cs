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
        protected Matrix[] _bonesTransforms;

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

        public override void UpdateBoundingVolumes()
        {
            // 1 - Global Bounding box
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            _model.CopyAbsoluteBoneTransformsTo(_bonesTransforms);

            // Update matrix world
            UpdateMatrix();
          
            // For each mesh of the model
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), _bonesTransforms[mesh.ParentBone.Index] * World);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            _boundingBox.Min = min;
            _boundingBox.Max = max;

            // 2 - Global bounding sphere
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
            _boundingSphere.Center = _position;
           
            // 3 - Update frustrum
            _boundingFrustrum = new BoundingFrustum(_camera.Projection * World);
        }

        public override void UpdateMatrix()
        {
            World = Matrix.CreateScale(Scale) *
                    Matrix.CreateRotationX(Rotation.X) *
                    Matrix.CreateRotationY(Rotation.Y) *
                    Matrix.CreateRotationZ(Rotation.Z) *
                    Matrix.CreateTranslation(Position);

            View = _camera.View;
        }

        #region GameState Pattern

        public override void LoadContent()
        {
            base.LoadContent();

            _model = YnG.Content.Load<Model>(_modelName);

            _bonesTransforms = new Matrix[_model.Bones.Count];

            UpdateBoundingVolumes();

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;
        }

        public override void Draw(GraphicsDevice device)
        {
            UpdateMatrix();
            
            _model.CopyAbsoluteBoneTransformsTo(_bonesTransforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();

                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1f, 0, 0);
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
                    effect.DirectionalLight0.Enabled = true;
                    
                    effect.AmbientLightColor = new Vector3(2f, 2f, 2f);
                    effect.EmissiveColor = new Vector3(0, 0.3f, 0.15f);

                    effect.World = _bonesTransforms[mesh.ParentBone.Index] * World;

                    effect.View = View;

                    effect.Projection = _camera.Projection;
                }
                mesh.Draw();
            }
        }

        #endregion

        public override string ToString()
        {
            return String.Format("[YnModel] {0}", _modelName);
        }
    }
}
