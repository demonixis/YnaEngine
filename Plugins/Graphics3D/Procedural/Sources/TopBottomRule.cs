using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics3D.Procedural
{
    /// <summary>
    /// Root rule used to process a cube volume
    /// </summary>
    public class TopBottomRule : ProcRule
    {
        private ProcRule _middle;

        public ProcRule MiddleRule { set { _middle = value; } }

        public TopBottomRule(ProcRuleset ruleset)
            : base(ruleset)
        {
        }

        public override void Process()
        {
            if (_middle != null)
            {
                // TODO process for each vertical face
                _middle.Process();
            }
        }

    }
}
