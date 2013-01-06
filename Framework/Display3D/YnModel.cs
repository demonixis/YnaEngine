using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Camera;
using Yna.Framework.Display3D.Lighting;
using Yna.Framework.Display3D.Material;

namespace Yna.Framework.Display3D
{
    public class YnModel : YnEntity3D
    {
        protected Model _model;
        protected string _modelName;
        protected Matrix[] _bonesTransforms;

        protected YnBasicLight _light; // Temp too

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
        public ModelMesh[] Meshes
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

        /// <summary>
        /// The model's asset name
        /// </summary>
        public string ModelName
        {
            get { return _modelName; }
        }

        public YnBasicLight Light
        {
            get { return _light; }
            set { _light = value; }
        }

        #endregion

        #region Constructor

        public YnModel(string modelName, Vector3 position)
            : base(position)
        {
            _modelName = modelName;
            _light = new YnBasicLight();
        }

        public YnModel(string modelName)
            : this(modelName, new Vector3(0.0f, 0.0f, 0.0f))
        {

        }

        #endregion

        #region Bounding volumes and light

#if MONOGAME
        // FIX : VertexBuffer.GetData crash with current MonoGame revision :/
        public override void UpdateBoundingVolumes()
        {
            _boundingBox.Min = new Vector3(X - 5, Y - 5, Z - 5);
            _boundingBox.Max = new Vector3(X + 5, Y + 5, Z + 5);

            _boundingSphere.Center = Position;
            _boundingSphere.Radius = 5;
        }
#else
        public override void UpdateBoundingVolumes()
        {
            // 1 - Global Bounding box
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            _model.CopyAbsoluteBoneTransformsTo(_bonesTransforms);

            // Update matrix world
            UpdateMatrices();

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
        }
#endif

        /// <summary>
        /// OH MY GOD THIS IS HORRIBLE !! but it's here just for a some time
        /// </summary>
        /// <param name="effect"></param>
        protected virtual void UpdateLights(BasicEffect effect)
        {
            effect.LightingEnabled = true;

            // Mesh color light
            effect.AmbientLightColor = _light.AmbientColor * _light.AmbientIntensity;
            effect.DiffuseColor = Color.White.ToVector3();
            effect.EmissiveColor = Color.White.ToVector3() * 0.5f;
            effect.SpecularColor = Color.Black.ToVector3();
            effect.Alpha = 1;

            StockMaterial.UpdateLights(effect, _light);
        }

        public override void UpdateMatrices()
        {
            World = Matrix.CreateScale(Scale) *
                Matrix.CreateFromYawPitchRoll(_rotation.Y, _rotation.X, _rotation.Z) *
                Matrix.CreateTranslation(Position);

            View = _camera.View;
        }

        #endregion

        #region GameState Pattern

        public override void LoadContent()
        {
            _model = YnG.Content.Load<Model>(_modelName);

            _bonesTransforms = new Matrix[_model.Bones.Count];

            UpdateBoundingVolumes();

            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;
        }

        public override void Draw(GraphicsDevice device)
        {
            UpdateMatrices();

            _model.CopyAbsoluteBoneTransformsTo(_bonesTransforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    UpdateLights(effect);

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
