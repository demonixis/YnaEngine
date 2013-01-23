using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Framework.Display3D.Light;

namespace Yna.Framework.Display3D.Primitive
{
    public class BasePrimitive : YnObject3D
    {
        #region Private declarations

        // Texture
        protected Texture2D _texture;
        protected string _textureName;
        protected Vector2 _textureRepeat;

        // Segments size
        protected Vector3 _segmentSizes;
        protected bool _useDefaultLightning;
        protected bool _useLightning;
        protected bool _useVertexColor;
        protected bool _useTexture;
        protected bool _constructed;

        // Update flags
        protected bool _needMatricesUpdate;
        protected bool _needShaderUpdate;
        protected bool _needLightUpdate;

        #endregion

        #region Basic properties

        /// <summary>
        /// True if the texture is loaded
        /// </summary>
        public bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// Gets the state of the geometry.
        /// true if constructed then false
        /// </summary>
        public bool Constructed
        {
            get { return _constructed; }
        }

        /// <summary>
        /// Enable or disable the default lightning on the shader
        /// </summary>
        public bool UseDefaultLightning
        {
            get { return _useDefaultLightning; }
            set
            {
                _useDefaultLightning = value;
                _needLightUpdate = true;
            }
        }

        /// <summary>
        /// Enable or disable the lightning 
        /// </summary>
        public bool UseLighning
        {
            get { return _useLightning; }
            set
            {
                _useLightning = value;
                _needLightUpdate = true;
            }
        }

        /// <summary>
        /// Enable or disable vertex color
        /// </summary>
        public bool UseVertexColor
        {
            get { return _useVertexColor; }
            set
            {
                _useVertexColor = value;
                _needShaderUpdate = true;
            }
        }

        /// <summary>
        /// Enable or disable texture
        /// </summary>
        public bool UseTexture
        {
            get { return _useTexture; }
            set
            {
                _useTexture = value;
                _needShaderUpdate = true;
            }
        }

        #endregion

        #region Properties for size

        /// <summary>
        /// Width of the primitive
        /// </summary>
        public new int Width
        {
            get { return (int)_width; }
            protected set { _width = (float)value; }
        }

        /// <summary>
        /// Height of the primitive
        /// </summary>
        public new int Height
        {
            get { return (int)_height; }
            protected set { _height = (float)value; }
        }

        /// <summary>
        /// Depth of the primitive
        /// </summary>
        public new int Depth
        {
            get { return (int)_depth; }
            protected set { _depth = (float)value; }
        }

        /// <summary>
        /// Gets or sets the segments size. It represent the space between two vertex.
        /// </summary>
        public Vector3 SegmentSizes
        {
            get { return _segmentSizes; }
            set { _segmentSizes = value; }
        }

        #endregion

        #region Properties for textures

        /// <summary>
        /// Texture to use with the terrain
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                _textureName = _texture.Name;
            }
        }

        /// <summary>
        /// Gets or sets the repeat value for the texture used by the object
        /// </summary>
        public Vector2 TextureRepeat
        {
            get { return _textureRepeat; }
            set { _textureRepeat = value; }
        }

        /// <summary>
        /// Gets or sets the texture name
        /// </summary>
        public string TextureName
        {
            get { return _textureName; }
            set { _textureName = value; }
        }

        #endregion

        #region Properties for flags

        /// <summary>
        /// Gets or sets the value of the flags who's used to update matrices.
        /// If set to true, all matrices will be updated
        /// </summary>
        public bool NeedMatricesUpdate
        {
            get { return _needMatricesUpdate; }
            set { _needMatricesUpdate = value; }
        }

        /// <summary>
        /// Gets or sets the value of the flags who's used to update the shader.
        /// If set to true the shader will be updated
        /// </summary>
        public bool NeedShaderUpdate
        {
            get { return _needShaderUpdate; }
            set { _needShaderUpdate = value; }
        }

        /// <summary>
        /// Gets or sets the value of the flags who's used to update lightning.
        /// If set to true lightning will be updated
        /// </summary>
        public bool NeedLightUpdate
        {
            get { return _needLightUpdate; }
            set { _needLightUpdate = value; }
        }

        #endregion

        #region Constructors

        public BasePrimitive()
            : this(new Vector3(0, 0, 0))
        {

        }

        public BasePrimitive(Vector3 position)
            : base(position)
        {
            _texture = null;
            _textureName = String.Empty;
            _textureRepeat = Vector2.One;

            _segmentSizes = Vector3.One;
            _useDefaultLightning = false;
            _useVertexColor = false;
            _useTexture = false;
            _constructed = false;

            _needMatricesUpdate = true;
            _needShaderUpdate = true;
            _needLightUpdate = true;
        }

        #endregion

        /// <summary>
        /// Setup the basic effet. Note that the flag NeedShaderUpdate must be set to
        /// true for updating. If you wan't to force update use true on parameter
        /// </summary>
        /// <param name="forceUpdate">True for force update</param>
        protected virtual void UpdateShader(bool forceUpdate)
        {
            if (_needShaderUpdate || forceUpdate)
            {
                if (_useDefaultLightning)
                    _basicEffect.EnableDefaultLighting();
                else
                    UpdateLightning(true);
                
                _basicEffect.VertexColorEnabled = _useVertexColor;

                if (_texture != null)
                    _basicEffect.Texture = _texture;

                _basicEffect.TextureEnabled = _useTexture;
            }

            _needShaderUpdate = false;
        }

        /// <summary>
        /// Setup the basic effet. Note that the flag NeedShaderUpdate must be set to
        /// true for updating.
        /// </summary>
        protected virtual void UpdateShader()
        {
            UpdateShader(false);
        }

        /// <summary>
        /// Set the lightning. Note that the flag NeedLightUpdate must be set to
        /// true for updating. If you wan't force update use true on parameter
        /// </summary>
        /// <param name="forceUpdate">True for force update</param>
        protected virtual void UpdateLightning(bool forceUpdate)
        {
            if (_needLightUpdate || forceUpdate)
            {
                _basicEffect.LightingEnabled = !_useDefaultLightning;
                _basicEffect.DirectionalLight0.Enabled = true;
                _basicEffect.DirectionalLight0.Direction = _light.Direction;
                _basicEffect.DirectionalLight0.DiffuseColor = _light.Diffuse;
                _basicEffect.DirectionalLight0.SpecularColor = _light.Specular;
                _basicEffect.AmbientLightColor = _light.Ambient;
                _basicEffect.EmissiveColor = _light.Emissive;
                _basicEffect.Alpha = _light.Alpha;
            }
        }

        /// <summary>
        /// Set the lightning. Note that the flag NeedLightUpdate must be set to
        /// true for updating. If you wan't force update use true on parameter
        /// </summary>
        public virtual void UpdateLightning()
        {
            UpdateLightning(false);
        }

        /// <summary>
        /// Update matrices world and view. There are 3 updates
        /// 1 - Scale
        /// 2 - Rotation on Y axis (override if you wan't more)
        /// 3 - Translation
        /// </summary>
        public override void UpdateMatrices()
        {
            World = Matrix.CreateScale(Scale) *
                Matrix.CreateRotationY(_rotation.Y) *
                Matrix.CreateTranslation(Position);

            View = _camera.View;
        }

        /// <summary>
        /// Update Bounding Box and Bounding Sphere
        /// </summary>
        public override void UpdateBoundingVolumes()
        {
            _boundingBox = new BoundingBox(
                new Vector3(X, Y, Z),
                new Vector3(X + Width, Y + Height, Z + Depth));

            _boundingSphere = new BoundingSphere(
                new Vector3(X + Width / 2, Y + Height / 2, Z + Depth / 2),
                Math.Max(Math.Max(Width, Height), Depth));
        }

        public virtual void PreDraw()
        {
            if (_needMatricesUpdate)
                UpdateMatrices();

            if (_dynamic)
                UpdateBoundingVolumes();

            if (_needShaderUpdate)
                UpdateShader();

            if (_needLightUpdate)
                UpdateLightning();

            _basicEffect.World = World;
            _basicEffect.View = View;
            _basicEffect.Projection = _camera.Projection;
        }

        /// <summary>
        /// Draw the object
        /// </summary>
        /// <param name="device"></param>
        public override void Draw(GraphicsDevice device)
        {
            PreDraw();
        }
    }
}
