using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Framework.Display3D.Renderer
{
    /// <summary>
    /// Class adapted from XNA Wiki http://www.xnawiki.com/index.php/Main_Page
    /// TODO : Create an instaciable object
    /// </summary>
    public static class BoundingBoxRenderer
    {
        #region Properties

        private static VertexPositionColor [] verts = new VertexPositionColor [8];
        private static short [] indices = new short []
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            0, 4,
            1, 5,
            2, 6,
            3, 7,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
        };

        private static BasicEffect effect;

        #endregion

        public static void Draw(BoundingBox box, GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(graphicsDevice);
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }

            Vector3 [] corners = box.GetCorners();
            for (int i = 0; i < 8; i++)
            {
                verts [i].Position = corners [i];
                verts [i].Color = color;
            }

            effect.View = view;
            effect.Projection = projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, verts, 0, 8, indices, 0, indices.Length / 2);
            }
        }
    }
}
