using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Procedural
{
    public class RepeatXRule : RepeatRule
    {
        public RepeatXRule(ProcRuleset ruleset) : base(ruleset) { }

        public override void Process()
        {
            Vector3 currentPosition = _position;
            int maxWidth = (int) _availableSpace.X;
            int currentWidth = 0;

            _repeatedRule.AvailableSpace = new Vector2(_repeatSize, _availableSpace.Y);
            while (currentWidth + _repeatSize <= maxWidth)
            {
                _repeatedRule.Angle = _angle;
                _repeatedRule.Position = currentPosition;
                _repeatedRule.Process();

                 if(_angle == 0)
                 {
                     currentPosition += new Vector3(0, 0, _repeatSize);
                 }
                 else if (_angle == 90)
                 {
                     currentPosition += new Vector3(_repeatSize, 0, 0);
                 }
                 else if (_angle == 180)
                 {
                     currentPosition += new Vector3(0, 0, -_repeatSize);
                 }
                 else if (_angle == 270)
                 {
                     currentPosition += new Vector3(-_repeatSize, 0, 0);
                 }

                
                currentWidth += _repeatSize;
            }
        }
    }
}
