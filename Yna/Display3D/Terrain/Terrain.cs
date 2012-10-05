using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Display3D;
using Yna.Display3D.Camera;

namespace Yna.Display3D.Terrain
{
    public abstract class Terrain : YnBase3D
    {
        protected int _width;
        protected int _height;
        protected int _depth;

        protected VertexPositionColor[] vertices;
        protected short[] _indexes;
        protected BasicEffect _basicEffect;

        protected BaseCamera _camera;

        public int Width
        {
            get { return _width; }
            protected set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            protected set { _height = value; }
        }

        public Terrain()
        {
            _camera = new FixedCamera();
        }

        public Terrain(BaseCamera camera)
        {
            _camera = camera;
        }

        protected abstract void CreateVertices();

        protected abstract void CreateIndex();

        public virtual void Draw(GraphicsDevice device)
        {
            _basicEffect.World = _camera.World; // Change this, it will be sceneGraph.World or other thing

            _basicEffect.View = _camera.View * 
                Matrix.CreateTranslation(Position) *
                Matrix.CreateScale(Scale) * 
                Matrix.CreateRotationX(Rotation.X) * 
                Matrix.CreateRotationY(Rotation.Y) * 
                Matrix.CreateRotationZ(Rotation.Z);

            _basicEffect.Projection = _camera.Projection;

            _basicEffect.LightingEnabled = false;

            _basicEffect.VertexColorEnabled = true;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, _indexes, 0, _indexes.Length / 3);
            }
        }
    }
}
