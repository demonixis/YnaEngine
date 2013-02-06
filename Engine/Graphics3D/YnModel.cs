﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    public class YnModel : YnEntity3D
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

        #endregion

        #region Constructor

        public YnModel(string modelName, Vector3 position)
            : base(position)
        {
            _modelName = modelName;
            _material = new BasicMaterial();
        }

        public YnModel(string modelName)
            : this(modelName, new Vector3(0.0f, 0.0f, 0.0f))
        {

        }

        #endregion

        #region Bounding volumes and light

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

        protected virtual void UpdateEffect(BasicEffect effect)
        {
            if (_material == null)
            {
                effect.LightingEnabled = true;

                // Mesh color light
                effect.AmbientLightColor = Color.White.ToVector3();
                effect.DiffuseColor = Color.White.ToVector3();
                effect.EmissiveColor = Color.White.ToVector3() * 0.5f;
                effect.SpecularColor = Color.Black.ToVector3();
                effect.Alpha = 1;
            }
            else
            {
                _material.Update(ref _world, ref _view, ref _projection, ref _position);

                BasicMaterial material = (BasicMaterial)_material;

                if (material != null)
                {
                    effect.Alpha = material.AlphaColor;
                    effect.AmbientLightColor = material.AmbientColor * material.AmbientIntensity;
                    effect.DiffuseColor = material.DiffuseColor * material.DiffuseIntensity;
                    effect.EmissiveColor = material.EmissiveColor * material.EmissiveIntensity;
                    effect.FogColor = material.FogColor;
                    effect.FogEnabled = material.EnableFog;
                    effect.FogStart = material.FogStart;
                    effect.FogEnd = material.FogEnd;
                    effect.LightingEnabled = true;

                    if (material.EnableDefaultLighting)
                        effect.EnableDefaultLighting();

                    effect.PreferPerPixelLighting = material.EnabledPerPixelLighting;
                    effect.SpecularColor = material.SpecularColor * material.SpecularIntensity;
                    effect.VertexColorEnabled = material.EnableVertexColor;


                    YnBasicLight light = (YnBasicLight)material.Light;

                    if (light != null)
                    {
                        StockMaterial.UpdateLighting(effect, light);
                        effect.AmbientLightColor *= light.AmbientColor * light.AmbientIntensity;
                    }
                }
            }
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

            _material.LoadContent();

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
                    UpdateEffect(effect);

                    effect.World = _bonesTransforms[mesh.ParentBone.Index] * World;

                    effect.View = _camera.View;

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