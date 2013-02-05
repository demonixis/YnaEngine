using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics3D.Geometry;

namespace Yna.Engine.Graphics3D.Procedural
{
    public class FaceRule : ProcRule
    {
        private ProcRule _mainRule;
        private ProcRule _edgeRule;

        public ProcRule MainRule { set { _mainRule = value; } }
        public ProcRule EdgeRule { set { _edgeRule = value; } }

        public FaceRule(ProcRuleset ruleset)
            : base(ruleset)
        {
        }

        public override void Process()
        {
            CubeGeometry volume = _ruleset.ProcVolume;

            if (_mainRule != null)
            {
                _mainRule.FitSize = new Vector3(volume.Width, volume.Height, volume.Depth);

                _mainRule.Angle = 0;
                _mainRule.AvailableSpace = new Vector2(volume.Depth, volume.Height);
                _mainRule.Position = new Vector3(
                    volume.X + volume.Width / 2,
                    volume.Y,
                    volume.Z - volume.Depth / 2
                );
                _mainRule.Process();

                _mainRule.Angle = 90;
                _mainRule.AvailableSpace = new Vector2(volume.Width, volume.Height);
                _mainRule.Position = new Vector3(
                    volume.X - volume.Width / 2,
                    volume.Y,
                    volume.Z - volume.Depth / 2
                );
                _mainRule.Process();


                _mainRule.Angle = 180;
                _mainRule.AvailableSpace = new Vector2(volume.Depth, volume.Height);
                _mainRule.Position = new Vector3(
                    volume.X - volume.Width / 2,
                    volume.Y,
                    volume.Z + volume.Depth / 2
                );
                _mainRule.Process();

                _mainRule.Angle = 270;
                _mainRule.AvailableSpace = new Vector2(volume.Width, volume.Height);
                _mainRule.Position = new Vector3(
                    volume.X + volume.Width / 2,
                    volume.Y,
                    volume.Z + volume.Depth / 2
                );
                _mainRule.Process();   
            }

            if (_edgeRule != null)
            {
                _edgeRule.Angle = 0;
                _edgeRule.AvailableSpace = new Vector2(0.8f, volume.Height);
                _edgeRule.Position = new Vector3(
                    volume.X + volume.Width / 2 + 0.8f / 4,
                    volume.Y,
                    volume.Z + volume.Depth / 2 - 0.8f / 4
                );
                _edgeRule.Process();

                _edgeRule.Angle = 90;
                _edgeRule.AvailableSpace = new Vector2(0.8f, volume.Height);
                _edgeRule.Position = new Vector3(
                    volume.X + volume.Width / 2 - 0.8f / 4,
                    volume.Y,
                    volume.Z - volume.Depth / 2 - 0.8f / 4
                );
                _edgeRule.Process();

                _edgeRule.Angle = 180;
                _edgeRule.AvailableSpace = new Vector2(0.8f, volume.Height);
                _edgeRule.Position = new Vector3(
                    volume.X - volume.Width / 2 - 0.8f / 4,
                    volume.Y,
                    volume.Z - volume.Depth / 2 + 0.8f / 4
                );
                _edgeRule.Process();

                _edgeRule.Angle = 270;
                _edgeRule.AvailableSpace = new Vector2(0.8f, volume.Height);
                _edgeRule.Position = new Vector3(
                    volume.X - volume.Width / 2 + 0.8f / 4,
                    volume.Y,
                    volume.Z + volume.Depth / 2 + 0.8f / 4
                );
                _edgeRule.Process();
            }
        }
    }
}
