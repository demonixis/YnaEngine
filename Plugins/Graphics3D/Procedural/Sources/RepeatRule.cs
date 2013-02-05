using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics3D.Procedural
{
    public abstract class RepeatRule : ProcRule
    {
        protected ProcRule _repeatedRule;
        protected int _repeatSize;
        protected int _faceSize;

        public ProcRule RepeatedRule { set { _repeatedRule = value; } }
        public int RepeatSize { set { _repeatSize = value; } }
        public int FaceSize { set { _faceSize = value; } }

        public RepeatRule(ProcRuleset ruleset) : base(ruleset) { }
    }
}
