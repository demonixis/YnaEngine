// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Yna.Engine.Graphics3D.Cameras;
using Yna.Engine.Graphics3D.Geometries;
using Yna.Engine.Graphics3D.Lighting;
using Yna.Engine.Graphics3D.Materials;

namespace Yna.Engine.Graphics3D.Terrains
{
    /// <summary>
    /// A Skybox for simulating boxed sky
    /// </summary>
    public class SkyBox : YnGroup3D
    {
        private string[] _textureNames;
        private Texture2D[] _textures;
        private YnEntity3D[] _walls;
        private FogData _fogData;

        public bool FogEnabled { get; set; }

        #region constructors

        /// <summary>
        /// Create a Skybox 3D object.
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size of skybox</param>
        /// <param name="textureNames">An array of textures in this order: negX/posX/negY/posY/negZ/posZ</param>
        public SkyBox(Vector3 position, float size, string[] textureNames)
            : base(null)
        {
            _textureNames = textureNames;
            _textures = new Texture2D[6];
            _walls = new YnMeshGeometry[6];
            _position = position;
            _width = size;
            _height = size;
            _depth = size;
        }

        /// <summary>
        /// Create a Skybox 3D object with just one texture.
        /// </summary>
        /// <param name="camera">Camera to use.</param>
        /// <param name="position">Position</param>
        /// <param name="size">Size of skybox</param>
        /// <param name="textureName">The texture to use on each face.</param>
        public SkyBox(Vector3 position, float size, string textureName)
            : this(position, size, new string[] { })
        {
            _textureNames = new string[6];

            for (var i = 0; i < 6; i++)
                _textureNames[i] = textureName;
        }

        /// <summary>
        /// Create a Skybox 3D object. Don't forget to attach a Camera.
        /// </summary>
        /// <param name="size">Size of skybox</param>
        /// <param name="textureNames">An array of textures in this order: negX/posX/negY/posY/negZ/posZ</param>
        public SkyBox(float size, string[] textureNames)
            : this(new Vector3(0.0f), size, textureNames)
        {
        }

        /// <summary>
        /// Create a Skybox 3D object with just one texture. Don't forget to attach a Camera.
        /// </summary>
        /// <param name="size">Size of skybox</param>
        public SkyBox(float size, string textureName)
            : this(new Vector3(0.0f), size, textureName)
        {
        }

        #endregion

        public override void LoadContent()
        {
            if (_textureNames.Length < 6)
                throw new Exception("[Skybox] The array must contains 6 names");

            var sizes = new Vector3(_width, 1, _depth);

            var positions = new Vector3[6]
            {
                new Vector3(X - Width, Y, Z),
                new Vector3(X + Width, Y, Z),
                new Vector3(X, Y - (Height), Z),
                new Vector3(X, Y + (Height), Z),
                new Vector3(X, Y, Z - Depth),
                new Vector3(X, Y, Z + Depth),
            };

            var rotations = new Vector3[6]
            {
                new Vector3(-MathHelper.PiOver2, 0.0f, -MathHelper.PiOver2),
                new Vector3(-MathHelper.PiOver2, 0.0f, MathHelper.PiOver2),
                new Vector3(0.0f),
                new Vector3(MathHelper.Pi, 0, 0.0f),
                new Vector3(-MathHelper.PiOver2, MathHelper.Pi, 0.0f),
                new Vector3(-MathHelper.PiOver2, 0.0f, 0.0f),
            };

            BasicMaterial material;

            for (int i = 0; i < 6; i++)
            {
                material = new BasicMaterial(_textureNames[i]);
                material.EnableDefaultLighting = false;
                material.EnableLighting = false;

                _walls[i] = new YnMeshGeometry(new PlaneGeometry(sizes), material);
                _walls[i].Rotation = rotations[i];
                _walls[i].Position = positions[i];
                Add(_walls[i]);
            }

            base.LoadContent();
        }

        public Material[] GetMaterials()
        {
            var materials = new Material[6];

            for (var i = 0; i < 6; i++)
                materials[i] = (this[i] as YnEntity3D).Material;

            return materials;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, Camera camera, SceneLight light, ref FogData fog)
        {
            if (FogEnabled)
                base.Draw(gameTime, device, camera, light, ref fog);
            else
                base.Draw(gameTime, device, camera, light, ref _fogData);
        }
    }
}
