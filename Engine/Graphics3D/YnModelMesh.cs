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
    public class YnModelMesh : YnMesh
    {
        private YnModel _model;

        public YnModelMesh(YnModel model)
        {
            _model = model;
            _material = new BasicMaterial();
        }

        public YnModelMesh(YnModel model, BaseMaterial material)
        {
            _model = model;
            _material = material;
            _model.SetEffect(_material.Effect);
        }

        public YnModelMesh(Model model, BaseMaterial material)
        {
            _model = new YnModel(model);
            _material = material;
        }

        public YnModelMesh(Model model)
            : this(model, null)
        {
            
        }
    }
}
