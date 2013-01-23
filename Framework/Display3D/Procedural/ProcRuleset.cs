using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Framework.Display3D.Primitive;
using Microsoft.Xna.Framework;

namespace Yna.Framework.Display3D.Procedural
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcRuleset
    {
        private CubeShape _procVolume; // The base volume
        private TopBottomRule _rootRule; // The root rule

        private YnGroup3D _procMesh; // The generated group

        public TopBottomRule RootRule { set { _rootRule = value; } }
        public CubeShape ProcVolume { get { return _procVolume; } }
        public YnGroup3D GeneratedMesh { get { return _procMesh; } }

        public ProcRuleset()
        {
            _procMesh = new YnGroup3D(null);
            _procVolume = new CubeShape("", new Vector3(5, 10, 5), new Vector3(0, 0, 0));
        }

        public void Generate()
        {
            _procMesh.Clear();
            _rootRule.Process();
        }

        public void AddMesh(YnModel mesh)
        {
            _procMesh.Add(mesh);
        }

        public void ResizeVolume(Vector3 size)
        {
            Vector3 pos = new Vector3(-size.X / 2, size.Y / 2, -size.Z / 2);
            _procVolume = new CubeShape("", size, pos);
        }
    }
}
