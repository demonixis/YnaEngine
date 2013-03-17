using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D
{
    /// <summary>
    /// A mesh who use a model loaded from content manager.
    /// </summary>
    public class YnMeshModel : YnMesh
    {
        protected YnModel _model;
        protected BaseMaterial[] _materials;

        /// <summary>
        /// Gets the model used by this mesh
        /// </summary>
        public YnModel Model
        {
            get { return _model; }
            protected set { _model = value; }
        }

        #region Constructors

        /// <summary>
        /// Create an empty mesh.
        /// </summary>
        public YnMeshModel()
        {
            _model = null;
            _material = null;
        }

        /// <summary>
        /// Create an YnMeshModel with a model loaded from content manager and a material.
        /// </summary>
        /// <param name="model">A Model instance</param>
        /// <param name="material">A material</param>
        public YnMeshModel(Model model, BaseMaterial material)
        {
            _model = new YnModel(model);
            _material = material;
        }

        /// <summary>
        /// Create an YnMeshModel with a model loaded from content manager and a BasicMaterial.
        /// </summary>
        /// <param name="model">A Model instance</param>
        public YnMeshModel(Model model)
            : this(model, null)
        {
            
        }

        /// <summary>
        /// Create an YnMeshModel with an YnModel and a BasicMaterial.
        /// </summary>
        /// <param name="model">An YnModel instance</param>
        /// <param name="material">A material</param>
        public YnMeshModel(YnModel model)
        {
            _model = model;
            _material = new BasicMaterial();
        }

        /// <summary>
        /// Create an YnMeshModel with an YnModel and a material.
        /// </summary>
        /// <param name="model">An YnModel instance</param>
        /// <param name="material">A material</param>
        public YnMeshModel(YnModel model, BaseMaterial material)
        {
            _model = model;
            _material = material;
            _model.SetEffect(_material.Effect);
        }

        #endregion

        /// <summary>
        /// Load model and material.
        /// </summary>
        public override void LoadContent()
        {
            _model.LoadContent();
            _material.LoadContent();
#if DEBUG
            if (_material == null)
            {
                _materials = new BaseMaterial[_model.Model.Meshes.Count];
                int counter = 0;

                foreach (ModelMesh mesh in _model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        BaseMaterial material = new BasicMaterial();

                        var effect = part.Effect as BasicEffect;

                        if (effect != null)
                        {
                            material.Texture = effect.Texture;
                            material.Effect = effect;
                        }

                        _materials[counter++] = material;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// Draw the model.
        /// </summary>
        /// <param name="device">GraphicsDevice</param>
        public override void Draw(GraphicsDevice device)
        {
            _model.Draw(device);
        }
    }
}
