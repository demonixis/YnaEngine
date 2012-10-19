using System;
using Microsoft.Xna.Framework;
using Yna.Display3D;

namespace Yna.Display3D
{
    public class YnG3
    {
        public static bool Collide(YnModel modelA, YnModel modelB)
        {
            bool collide = false;

            int countMeshA = modelA.Model.Meshes.Count;
            int countMeshB = modelB.Model.Meshes.Count;

            for (int i = 0; i < countMeshA; i++)
            {
                BoundingSphere meshABS = modelA.Model.Meshes[i].BoundingSphere;
                meshABS.Center += modelA.Position;

                for (int j = 0; j < countMeshB; j++)
                {
                    BoundingSphere meshBBS = modelB.Model.Meshes[j].BoundingSphere;
                    meshBBS.Center += modelB.Position;

                    if (meshABS.Intersects(meshBBS))
                        collide = true;
                }
            }

            return collide;
        }

        public static bool CollideWithGroup(YnModel model, YnGroup3D group)
        {
            bool collide = false;

            int groupSize = group.Count;

            for (int i = 0; i < groupSize; i++)
            {
                if (group[i] is YnModel)
                {
                    if (Collide(model, group[i] as YnModel))
                        collide = true;
                }
            }

            return collide;
        }
    }
}
