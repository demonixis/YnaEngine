using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Engine.Graphics3D.Procedural
{
    public class MeshRule : ProcRule
    {
        private List<string> _models;

        public MeshRule(ProcRuleset ruleset)
            : base(ruleset)
        {
            _models = new List<string>();
        }

        public override void Process()
        {
            CubeGeometry volume = _ruleset.ProcVolume;
            YnModel model = getModel();

            // Compute model scale to fit in the defined space
            float scaleX = 1f;
            float scaleZ = 1;
            
            if(_angle == 0 || _angle == 180)
            {
                scaleX = _availableSpace.X / model.Width;
                scaleZ = _availableSpace.X / model.Depth;
            }
            else
            {
                scaleX = _availableSpace.X / model.Width;
                scaleZ = _availableSpace.X / model.Depth;
            }
            
            Vector3 scale = new Vector3(
                scaleX,
                _availableSpace.Y / model.Height,
                scaleZ
            );

            model.Position = _position;
            model.RotateY(_angle);
            model.Scale = scale;
            
        }

        public void Add(string model)
        {
            _models.Add(model);
        }

        private YnModel getModel()
        {
            YnModel model = new YnModel(_models[0]);
            _ruleset.AddMesh(model);
            return model;
        }
    }
}
