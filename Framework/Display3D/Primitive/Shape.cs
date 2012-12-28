using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display3D.Primitive
{
    public abstract class Shape<T> : BasePrimitive where T : struct, IVertexType
    {
        protected T[] _vertices;
        protected ushort[] _indices;
        protected VertexBuffer _vertexBuffer;
        protected IndexBuffer _indexBuffer;

        public Shape()
        {
            
        }

        public abstract void CreateVertices();

        public abstract void CreateIndices();

        public virtual void CreateBuffers()
        {
            _vertexBuffer = new VertexBuffer(YnG.GraphicsDevice, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(_vertices);

            _indexBuffer = new IndexBuffer(YnG.GraphicsDevice, IndexElementSize.SixteenBits, _indices.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(_indices);
        }

        public override void Draw(GraphicsDevice device)
        {
            base.Draw(device);

            device.SetVertexBuffer(_vertexBuffer);
            device.Indices = _indexBuffer;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, _vertices.Length / 3);
            }

            device.SetVertexBuffer(null);
            device.Indices = null;
        }
    }
}
