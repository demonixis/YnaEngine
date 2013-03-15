using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Yna.Engine.Graphics3D.Camera;
using Yna.Engine.Graphics3D.Geometry;
using Yna.Engine.Graphics3D.Material;

namespace Yna.Engine.Graphics3D.Terrain
{
    public class SkyBox : YnGroup3D
    {
        private string[] _textureNames;
        private Texture2D[] _textures;
        private YnMesh[] _walls;

        /// <summary>
        /// Create a Skybox 3D object.
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="parent">Parent entity</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size of skybox</param>
        /// <param name="textureNames">An array of textures in this order: negX/posX/negY/posY/negZ/posZ</param>
        public SkyBox(BaseCamera camera, YnEntity3D parent, Vector3 position, float size, string[] textureNames)
            : base(camera, parent)
        {
            if (textureNames.Length < 6)
                throw new Exception("[Skybox] The array must contains 6 names");

            _textureNames = textureNames;
            _textures = new Texture2D[6];
            _walls = new YnGeometryMesh<VertexPositionNormalTexture>[6];
            _position = position;
            _width = size;
            _height = size;
            _depth = size;
        }

        public override void LoadContent()
        {
            Vector3 sizes = new Vector3(_width, 1, _depth);
           
            Vector3[] positions = new Vector3[6]
            {
                new Vector3(X, Y, Z - Depth),
                new Vector3(X, Y, Z + Depth),
                new Vector3(X, Y - (Height), Z),
                new Vector3(X, Y + (Height), Z),
                new Vector3(X - Width, Y, Z),
                new Vector3(X + Width, Y, Z),
            };

            Vector3[] rotations = new Vector3[6]
            {
                new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0.0f),
                new Vector3(-MathHelper.PiOver2, 0.0f, 0.0f),
                new Vector3(0.0f),
                new Vector3(MathHelper.Pi, MathHelper.Pi, 0.0f),
                new Vector3(-MathHelper.PiOver2, 0.0f, -MathHelper.PiOver2),
                new Vector3(-MathHelper.PiOver2, 0.0f, MathHelper.PiOver2)
            };

            for (int i = 0; i < 6; i++)
            {
                _walls[i] = new YnGeometryMesh<VertexPositionNormalTexture>(new PlaneGeometry(sizes), new BasicMaterial(_textureNames[i]));
                _walls[i].Rotation = rotations[i];
                Add(_walls[i]);
            }

            base.LoadContent();
        }
    }
}
