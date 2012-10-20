using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D;
using Yna.Display3D.Camera;

namespace Yna.Display3D
{
    public struct CollideInformation
    {
        public float Distance;
        public YnObject3D Object3D;
    }

    public class YnG3
    {
        #region Coordinates 2D/3D

        /// <summary>
        /// Get 2D position (on screen) from 3D position (on world)
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="position">Position 3D</param>
        /// <returns>The screen position</returns>
        public static Vector2 WorldToScreen(BaseCamera camera, ref Vector3 position)
        {
            Vector3 p2d = YnG.GraphicsDevice.Viewport.Project(position, camera.Projection, camera.View, Matrix.Identity);

            return new Vector2(p2d.X, p2d.Y);
        }

        /// <summary>
        /// Get 3D world position of from the 2D position (on screen)
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="position">Position on world</param>
        /// <returns>Position on 3D world</returns>
        public static Vector3 ScreenToWorld(BaseCamera camera, ref Vector2 position)
        {
            Vector3 p3d = YnG.GraphicsDevice.Viewport.Unproject(new Vector3(position, 0.0f), camera.Projection, camera.View, Matrix.Identity);

            return p3d;
        }

        #endregion

        #region Collision detection

        /// <summary>
        /// Get a Ray from the mouse coordinate
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <returns>A ray</returns>
        public static Ray GetMouseRay(BaseCamera camera)
        {
            Vector3 nearPoint = new Vector3(YnG.Mouse.Position, 0);
            Vector3 farPoint = new Vector3(YnG.Mouse.Position, 1);

            nearPoint = YnG.GraphicsDevice.Viewport.Unproject(nearPoint, camera.Projection, camera.View, Matrix.Identity);
            farPoint = YnG.GraphicsDevice.Viewport.Unproject(farPoint, camera.Projection, camera.View, Matrix.Identity);

            // Get the direction
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }

        /// <summary>
        /// Get the distance between the mouse cursor and an object3D
        /// </summary>
        /// <param name="camera">Camera to use</param>
        /// <param name="object3D">Object3D</param>
        /// <returns>The distance between the object and the mouse cursor, -1 if not collide</returns>
        public static float MouseCollideWithObject(BaseCamera camera, YnObject3D object3D)
        {
            float? distance = GetMouseRay(camera).Intersects(object3D.BoundingSphere);

            return distance != null ? (float)distance : -1.0f;
        }

        public static CollideInformation[] MouseCollideWithGroup(BaseCamera camera, YnGroup3D group)
        {
            List<CollideInformation> collides = new List<CollideInformation>();

            int groupSize = group.Count;

            for (int i = 0; i < groupSize; i++)
            {
                float distance = MouseCollideWithObject(camera, group[i]);

                if (distance > -1)
                    collides.Add(new CollideInformation() { Object3D = group[i], Distance = distance });
            }

            return collides.ToArray();
        }

        /// <summary>
        /// Test if two models colliding
        /// </summary>
        /// <param name="modelA">First model</param>
        /// <param name="modelB">Second model</param>
        /// <returns>True if modelA collinding modelB else false</returns>
        public static bool Collide(YnModel modelA, YnModel modelB)
        {
            bool collide = false;
            int j = 0;

            int countMeshA = modelA.Model.Meshes.Count;
            int countMeshB = modelB.Model.Meshes.Count;

            float maxScaleA = Math.Max(Math.Max(modelA.Scale.X, modelA.Scale.Y), modelA.Scale.Z);
            float maxScaleB = Math.Max(Math.Max(modelB.Scale.X, modelB.Scale.Y), modelB.Scale.Z);

            for (int i = 0; i < countMeshA; i++)
            {
                BoundingSphere meshABS = modelA.Model.Meshes[i].BoundingSphere;
                meshABS.Radius *= maxScaleA;
                meshABS.Center += modelA.Position;

                while (j < countMeshB && !collide)
                {
                    BoundingSphere meshBBS = modelB.Model.Meshes[j].BoundingSphere;
                    meshBBS.Radius *= maxScaleB;
                    meshBBS.Center += modelB.Position;

                    if (meshABS.Intersects(meshBBS))
                        collide = true;

                    j++;
                }
            }

            return collide;
        }

        /// <summary>
        /// Test if the model colliding another objects
        /// </summary>
        /// <param name="model">The model</param>
        /// <param name="group">A collection of models</param>
        /// <returns>Array of models that collides with model</returns>
        public static YnModel[] CollideWithGroup(YnModel model, YnGroup3D group)
        {
            List<YnModel> collides = new List<YnModel>();

            int groupSize = group.Count;

            for (int i = 0; i < groupSize; i++)
            {
                if (group[i] is YnModel)
                {
                    if (Collide(model, group[i] as YnModel))
                        collides.Add(group[i] as YnModel);
                }
            }

            return collides.ToArray();
        }

        #endregion

        #region Compute bounding box

        /// <summary>
        /// Get the bounding box of a loaded model
        /// </summary>
        /// <param name="model">An instance of model</param>
        /// <returns>The bounding box for the model</returns>
        public static BoundingBox CreateBoundingBox(Model model)
        {
            Vector3 modelMin = new Vector3(float.MaxValue);
            Vector3 modelMax = new Vector3(float.MinValue);

            Matrix[] _transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(_transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                Vector3 meshMax = new Vector3(float.MinValue);
                Vector3 meshMin = new Vector3(float.MaxValue);

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                    byte[] vertexData = new byte[stride * meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(meshPart.VertexOffset * stride, vertexData, 0, meshPart.NumVertices, 1);

                    Vector3 vertexPosition = new Vector3();

                    for (int i = 0, l = vertexData.Length; i < l; i += stride)
                    {
                        vertexPosition.X = BitConverter.ToSingle(vertexData, i);
                        vertexPosition.Y = BitConverter.ToSingle(vertexData, i + sizeof(float));
                        vertexPosition.Z = BitConverter.ToSingle(vertexData, i + sizeof(float) * 2);

                        meshMin = Vector3.Min(meshMin, vertexPosition);
                        meshMax = Vector3.Max(meshMax, vertexPosition);
                    }
                }

                meshMin = Vector3.Transform(meshMin, _transforms[mesh.ParentBone.Index]);
                meshMax = Vector3.Transform(meshMax, _transforms[mesh.ParentBone.Index]);

                modelMin = Vector3.Min(modelMin, meshMin);
                modelMax = Vector3.Max(modelMax, meshMax);
            }

            return new BoundingBox(modelMin, modelMax);
        }

        #endregion
    }
}
