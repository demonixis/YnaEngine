using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics3D.Procedural
{
    /// <summary>
    /// Procedural generation rule. A rule execute custom generation process and can be chained with other rules.
    /// </summary>
    public abstract class ProcRule
    {
        /// <summary>
        /// The ruleset reference
        /// </summary>
        protected ProcRuleset _ruleset;
        protected float _angle;
        protected Vector3 _position;
        protected Vector3 _fitSize;
        protected Vector2 _availableSpace;

        public float Angle { set { _angle = value; } }
        public Vector3 Position { set { _position = value; } }
        public Vector3 FitSize { set { _fitSize = value; } }
        public Vector2 AvailableSpace { set { _availableSpace = value; } }

        public ProcRule(ProcRuleset ruleset)
        {
            _ruleset = ruleset;
        }

        /// <summary>
        /// Process the rule and all sub rules
        /// </summary>
        public abstract void Process();
    }
}
