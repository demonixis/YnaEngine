﻿// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh who use a model loaded from content manager.
    /// </summary>
    public class YnModel : YnEntity3D
    {
        private string _modelName;
        protected Model _model;
        protected Matrix[] _bonesTransforms;
        protected Material[] _materials;

        /// <summary>
        /// Gets the model used by this mesh
        /// </summary>
        public Model Model
        {
            get { return _model; }
            protected set { _model = value; }
        }

        #region Constructors

        /// <summary>
        /// Create an empty mesh.
        /// </summary>
        public YnModel()
        {
            _model = null;
            _material = null;
            _materials = null;
            _modelName = String.Empty;
            _bonesTransforms = null;
        }

        /// <summary>
        /// Create an YnEntity3DModel with a model loaded from content manager and a material.
        /// </summary>
        /// <param name="model">A Model instance</param>
        /// <param name="material">A material</param>
        public YnModel(Model model, Materials.Material material)
            : this()
        {
            _model = model;
            _material = material;
            _modelName = _model.ToString();
        }

        /// <summary>
        /// Create an YnEntity3DModel with a model loaded from content manager and a BasicMaterial.
        /// </summary>
        /// <param name="model">A Model instance</param>
        public YnModel(Model model)
            : this(model, null)
        {

        }

        /// <summary>
        /// Create an YnEntity3DModel with an YnModel and a material.
        /// </summary>
        /// <param name="model">An YnModel instance</param>
        /// <param name="material">A material</param>
        public YnModel(string modelName, Materials.Material material)
            : this()
        {
            _model = null;
            _modelName = modelName;
            _material = material;
        }

        /// <summary>
        /// Create an YnEntity3DModel.
        /// </summary>
        /// <param name="modelName">Model name.</param>
        public YnModel(string modelName)
            : this(modelName, new BasicMaterial())
        {

        }

        #endregion

        /// <summary>
        /// Update bounding box and bounding sphere. Width, Height and Depth properties are updated too.
        /// </summary>
        public override void UpdateBoundingVolumes()
        {
            // 1 - Global Bounding box
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

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

            _boundingBox.Min = min *= _scale;
            _boundingBox.Max = max *= _scale;

            // 2 - Global bounding sphere
            _width = _boundingBox.Max.X - _boundingBox.Min.X;
            _height = _boundingBox.Max.Y - _boundingBox.Min.Y;
            _depth = _boundingBox.Max.Z - _boundingBox.Min.Z;

            _boundingSphere.Radius = Math.Max(Math.Max(_width, _height), _depth) / 2;
            _boundingSphere.Center = _position;
        }

        /// <summary>
        /// Update model effect
        /// </summary>
        /// <param name="meshEffect">An effect</param>
        protected virtual void UpdateModelEffects(Effect meshEffect, SceneLight light, ref FogData fog)
        {
            if (meshEffect is BasicEffect)
            {
                var effect = meshEffect as BasicEffect;

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
                    var material = (BasicMaterial)_material;
                    if (material != null)
                        UpdateEffect(effect, material, light, ref fog);
                }
            }
        }

        /// <summary>
        /// Update the current effect. This method will change.
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="material"></param>
        private void UpdateEffect(BasicEffect effect, BasicMaterial material, SceneLight light, ref FogData fog)
        {
            effect.Alpha = material.AlphaColor;
            effect.AmbientLightColor = light.AmbientColor * light.AmbientIntensity;
            effect.DiffuseColor = material.DiffuseColor * material.DiffuseIntensity;
            effect.EmissiveColor = material.EmissiveColor * material.EmissiveIntensity;
            effect.FogColor = fog.Color;
            effect.FogEnabled = fog.Enabled;
            effect.FogStart = fog.Start;
            effect.FogEnd = fog.End;
            effect.LightingEnabled = true;

            if (material.EnableDefaultLighting)
                effect.EnableDefaultLighting();

            effect.PreferPerPixelLighting = material.PreferPerPixelLighting;
            effect.SpecularColor = material.SpecularColor * material.SpecularIntensity;
            effect.VertexColorEnabled = material.EnableVertexColor;

            if (light != null)
            {
                StockMaterial.UpdateLighting(effect, light);
                effect.AmbientLightColor *= light.AmbientColor * light.AmbientIntensity;
            }
        }

        /// <summary>
        /// Change all effects to the model and replace them.
        /// </summary>
        /// <param name='effect'>A custom effect<param>
        public void SetEffect(Effect effect)
        {
            foreach (ModelMesh mesh in _model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect = effect;
        }

        /// <summary>
        /// Gets material used by the model.
        /// </summary>
        /// <returns>An array of material used by the model.</returns>
        public Material[] GetModelMaterial()
        {
            var materials = new List<Material>();
            Material material = null;

            foreach (var mesh in _model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    material = new BasicMaterial();

                    var effect = part.Effect as BasicEffect;

                    if (effect != null)
                    {
                        material.Texture = effect.Texture;
                        material.Effect = effect;
                    }

                    materials.Add(material);
                }
            }

            return materials.ToArray();
        }

        /// <summary>
        /// Load model and material.
        /// </summary>
        public override void LoadContent()
        {
            if (_model == null)
                _model = YnG.Content.Load<Model>(_modelName);

            _bonesTransforms = new Matrix[_model.Bones.Count];

            UpdateBoundingVolumes();

            if (_material == null)
                _materials = GetModelMaterial();
            else
                _material.LoadContent();
        }

        /// <summary>
        /// Draw the model.
        /// </summary>
        /// <param name="device">GraphicsDevice</param>
        public override void Draw(GameTime gameTime, GraphicsDevice device, Camera camera, SceneLight light, ref FogData fog)
        {
            UpdateMatrix();

            _material.Update(camera, light, ref _world, ref fog);
            _model.CopyAbsoluteBoneTransformsTo(_bonesTransforms);

            foreach (var mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    UpdateModelEffects(effect, light, ref fog);
                    effect.World = _bonesTransforms[mesh.ParentBone.Index] * World;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}
