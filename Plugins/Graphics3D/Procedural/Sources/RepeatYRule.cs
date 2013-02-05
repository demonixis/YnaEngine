using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Procedural
{
    public class RepeatYRule : RepeatRule
    {
        public RepeatYRule(ProcRuleset ruleset) : base(ruleset) { }

        public override void Process()
        {
            Vector3 currentPosition = new Vector3(_position.X, _position.Y - _ruleset.ProcVolume.Height / 2 + (float)_repeatSize/2, _position.Z);
            int maxHeight = (int)_availableSpace.Y;
            int currentHeight = 0;

            _repeatedRule.AvailableSpace = new Vector2(_availableSpace.X, _repeatSize);
            while (currentHeight + _repeatSize <= maxHeight)
            {
                _repeatedRule.Angle = _angle;
                _repeatedRule.Position = currentPosition;
                _repeatedRule.Process();

                currentPosition += new Vector3(0, _repeatSize, 0);
                currentHeight += _repeatSize;
            }
        }
    }
}
