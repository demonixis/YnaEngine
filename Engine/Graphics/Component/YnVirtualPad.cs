// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Graphics.Event;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Component
{
    public enum VirtualPadSize
    {
        Small = 0, Normal, Big
    }

    public enum PadButtons
    {
        Up = 0, Down, Left, Right, StrafeLeft, StrafeRight, ButtonA, ButtonB, Pause, None
    }

    /// <summary>
    /// A graphic virtual pad component that you can add to a scene and use for moving an object/camera
    /// </summary>
    public class YnVirtualPad : YnGroup
    {
        // Direction
        private YnSprite _upPad;
        private YnSprite _downPad;
        private YnSprite _leftPad;
        private YnSprite _rightPad;
        private YnSprite _strafeLeftPad;
        private YnSprite _strafeRightPad;

        // Buttons
        private YnSprite _buttonActionA;
        private YnSprite _buttonActionB;
        private YnSprite _buttonPause;

        // Dimenssion
        private Vector2 _margin;
        private Vector2 _padding;
        private Rectangle _inputRectangle;
        private int _spaceBetweenActionButtons;

        // Enable or disable buttons
        private bool _enableStrafeButtons;
        private bool _enableButtonA;
        private bool _enableButtonB;
        private bool _enableButtonPause;
        private bool _inverseDirectionStrafe;

        // Textures informations
        private string [] _dpadTextureNames;
        private string[] _buttonsTexutreNames;

        #region Properties

        /// <summary>
        /// Gets or sets the scale value
        /// </summary>
        public new Vector2 Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;

                foreach (YnEntity entity in this)
                    entity.Scale = value;
            }
        }

        /// <summary>
        /// Gets or sets the padding value
        /// </summary>
        public Vector2 Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;

                if (_assetLoaded)
                    UpdateLayout();
            }
        }

        /// <summary>
        /// Gets or sets the margin value
        /// </summary>
        public Vector2 Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;

                if (_assetLoaded)
                    UpdateLayout();
            }
        }

        /// <summary>
        /// Gets or sets the space between the action buttons
        /// </summary>
        public int ButtonSpace
        {
            get { return _spaceBetweenActionButtons; }
            set
            {
                _spaceBetweenActionButtons = value;

                if (_assetLoaded)
                    UpdateLayout();
            }
        }

        /// <summary>
        /// Enable or disable strafe button
        /// </summary>
        public bool EnableStrafeButtons
        {
            get { return _enableStrafeButtons; }
            set
            {
                _enableStrafeButtons = value;
                _strafeLeftPad.Active = value;
                _strafeRightPad.Active = value;
            }
        }

        /// <summary>
        /// Enable or disable the first action button
        /// </summary>
        public bool EnabledButtonA
        {
            get { return _enableButtonA; }
            set
            {
                _enableButtonA = value;
                _buttonActionA.Active = value;

            }
        }

        /// <summary>
        /// Enable or disable this second action button
        /// </summary>
        public bool EnabledButtonB
        {
            get { return _enableButtonB; }
            set
            {
                _enableButtonB = value;
                _buttonActionB.Active = value;

            }
        }

        /// <summary>
        /// Enable or disable the pause/menu button
        /// </summary>
        public bool EnabledButtonPause
        {
            get { return _enableButtonPause; }
            set
            {
                _enableButtonPause = value;
                _buttonPause.Active = value;
            }
        }

        /// <summary>
        /// Inverse the strafe and left/right buttons
        /// </summary>
        public bool InverseDirectionStrafe
        {
            get { return _inverseDirectionStrafe; }
            set { _inverseDirectionStrafe = value; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when a button is pressed
        /// </summary>
        public event EventHandler<VirtualPadPressedEventArgs> VirtualPadPressed = null;

        /// <summary>
        /// Triggered when a button is released
        /// </summary>
        public event EventHandler<VirtualPadPressedEventArgs> VirtualPadReleased = null;

        private void OnPressed(VirtualPadPressedEventArgs e)
        {
            if (VirtualPadPressed != null)
                VirtualPadPressed(this, e);
        }

        private void OnReleased(VirtualPadPressedEventArgs e)
        {
            if (VirtualPadReleased != null)
                VirtualPadReleased(this, e);
        }

        #endregion

        /// <summary>
        /// Create a new Virtual pad
        /// </summary>
        public YnVirtualPad()
        {
            InitializeDefault();
            InitializeWithoutTextures();
            InitializeButtons();
        }

        /// <summary>
        /// Create a new virtual pad with textures for buttons.
        /// The array for dpad must contains at last 4 texture names (for dpad without strafe) or 6 texture names (for strafe)
        /// The array for buttons must contains at last 1 texture name (begining for buttonA) and 3 max
        /// Order for dpad : Up, Down, Left, Right, StrafeLeft, StrafeRight
        /// Order for buttons : ButtonA, ButtonB, ButtonPause
        /// </summary>
        /// <param name="textureNames"></param>
        public YnVirtualPad(string [] dpadTextureNames, string[] buttonTextureNames)
            : base()
        {
            _dpadTextureNames = dpadTextureNames;
            _buttonsTexutreNames = buttonTextureNames;
        }

        /// <summary>
        /// Initialize with default generated textures
        /// </summary>
        protected void InitializeWithoutTextures()
        {
            Color normal = Color.LightSteelBlue;
            Color strafe = Color.LightSlateGray;

            _upPad = new YnSprite(_inputRectangle, normal);
            _downPad = new YnSprite(_inputRectangle, normal);
            _leftPad = new YnSprite(_inputRectangle, strafe);
            _rightPad = new YnSprite(_inputRectangle, strafe);

            _strafeLeftPad = new YnSprite(_inputRectangle, normal);
            _strafeRightPad = new YnSprite(_inputRectangle, normal);

            _buttonActionA = new YnSprite(_inputRectangle, Color.Crimson);
            _buttonActionB = new YnSprite(_inputRectangle, Color.SpringGreen);
            _buttonPause = new YnSprite(_inputRectangle, Color.SlateBlue);
        }

        /// <summary>
        /// Initialize with default margin and padding
        /// </summary>
        protected void InitializeDefault()
        {
            _margin = new Vector2(3, 3);
            _padding = new Vector2(5, 5);
            _inputRectangle = new Rectangle(0, 0, 65, 65);
#if WINDOWS_PHONE
            _inverseDirectionStrafe = true;
#else
            _inverseDirectionStrafe = false;
#endif
            _spaceBetweenActionButtons = 45;
        }

        /// <summary>
        /// Sets textures for the digital pad
        /// The array of Texture2D must contains 6 textures
        /// Order : up, down, left, right, strafeLeft, strafeRight
        /// </summary>
        /// <param name="textures"></param>
        public void SetDPadTextures(Texture2D[] textures)
        {
            if (textures.Length >= 4)
            {
                _upPad.Texture = textures[0];
                _downPad.Texture = textures[1];
                _leftPad.Texture = textures[2];
                _rightPad.Texture = textures[3];

                if (textures.Length == 6)
                {
                    _strafeLeftPad.Texture = textures[4];
                    _strafeRightPad.Texture = textures[5];
                }
                else
                {
                    _strafeLeftPad.Active = false;
                    _strafeRightPad.Active = false;
                }

                if (_assetLoaded)
                    UpdateLayout();
            }
        }

        /// <summary>
        /// Sets textures for buttons A, B and pause
        /// The array of Texture2D must contains 3 textures
        /// Order : ButtonA, ButtonB, Pause
        /// </summary>
        /// <param name="textures"></param>
        public void SetButtonsTextures(Texture2D[] textures)
        {
            if (textures.Length == 3)
            {
                _buttonActionA.Texture = textures[0];
                _buttonActionB.Texture = textures[1];
                _buttonPause.Texture = textures[2];

                if (_assetLoaded)
                    UpdateLayout();
            }
        }

        /// <summary>
        /// Initialize buttons and add it to the group
        /// </summary>
        protected void InitializeButtons()
        {
            _upPad.Name = "Button_" + ((int)PadButtons.Up).ToString();
            Add(_upPad);

            _downPad.Name = "Button_" + ((int)PadButtons.Down).ToString();
            Add(_downPad);

            _strafeLeftPad.Name = "Button_" + ((int)PadButtons.StrafeLeft).ToString();
            Add(_strafeLeftPad);

            _strafeRightPad.Name = "Button_" + ((int)PadButtons.StrafeRight).ToString();
            Add(_strafeRightPad);

            _leftPad.Name = "Button_" + ((int)PadButtons.Left).ToString();
            Add(_leftPad);

            _rightPad.Name = "Button_" + ((int)PadButtons.Right).ToString();
            Add(_rightPad);

            _buttonActionA.Name = "Button_" + ((int)PadButtons.ButtonA).ToString();
            Add(_buttonActionA);

            _buttonActionB.Name = "Button_" + ((int)PadButtons.ButtonB).ToString();
            Add(_buttonActionB);

            _buttonPause.Name = "Button_" + ((int)PadButtons.Pause).ToString();
            Add(_buttonPause);

            //X = (int)(10 + _margin.X);
            //Y = (int)(YnG.Height - (2 * _downPad.Height) - (2 * _margin.Y));

            _alpha = 0.75f;

            foreach (YnSprite sprite in this)
            {
                sprite.MouseClick += Pad_Click;
                sprite.MouseReleased += Pad_Released;
                sprite.Alpha = _alpha;
            }
        }

        /// <summary>
        /// Load assets
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();

            if (_dpadTextureNames != null)
            {
                Texture2D[] dpadTextures = new Texture2D[_dpadTextureNames.Length];

                for (int i = 0; i < _dpadTextureNames.Length; i++)
                    dpadTextures[i] = YnG.Content.Load<Texture2D>(_dpadTextureNames[i]);

                SetDPadTextures(dpadTextures);
            }

            if (_buttonsTexutreNames != null)
            {
                Texture2D [] buttonTextures = new Texture2D[_buttonsTexutreNames.Length];

                for (int i = 0; i < _buttonsTexutreNames.Length; i++)
                    buttonTextures[i] = YnG.Content.Load<Texture2D>(_buttonsTexutreNames[i]);

                SetButtonsTextures(buttonTextures);
            }

            UpdateLayout();

            Width = (int)(_leftPad.Width + _upPad.Width + _rightPad.Width + 2 * _margin.X);
            Height = (int)(_leftPad.Height + _strafeLeftPad.Height + _margin.Y);

            _assetLoaded = true;
        }

        /// <summary>
        /// Update the layout. Call it if you change the skin after assets loading
        /// </summary>
        public virtual void UpdateLayout()
        {
            _leftPad.Move(_padding.X, YnG.Height - _leftPad.Height * 2 - _padding.Y * 2);
            _upPad.Move(_leftPad.X + _leftPad.Width + _margin.X, _leftPad.Y);
            _rightPad.Move(_upPad.X + _upPad.Width + _margin.X, _leftPad.Y);

            _strafeLeftPad.Move(_padding.X, (_upPad.Y + _upPad.Height) + _margin.Y);
            _downPad.Move(_strafeLeftPad.X + _strafeLeftPad.Width + _margin.X, _strafeLeftPad.Y);
            _strafeRightPad.Move(_downPad.X + _downPad.Width + _margin.X, _strafeLeftPad.Y);

            _buttonActionA.Move(YnG.Width - ((2 * _inputRectangle.Width) + _padding.X + _margin.X + _spaceBetweenActionButtons), _downPad.Y);
            _buttonActionB.Move(_buttonActionA.X + _buttonActionA.Width + _margin.X + _spaceBetweenActionButtons, _downPad.Y);
            _buttonPause.Move(_padding.X, _padding.Y);
        }

        /// <summary>
        /// Get the direction of a pressed button
        /// </summary>
        /// <param name="buttonName">A button</param>
        /// <returns>The button id</returns>
        protected PadButtons GetDirection(string buttonName)
        {
            string[] temp = buttonName.Split(new char[] { '_' });

            // The real index of the button
            int index = int.Parse(temp[1].ToString());

            // Define used index
            int usedIndex = index;

            // If strafe and direction are inversed we change the index
            if (_inverseDirectionStrafe && (index == 2 || index == 3 || index == 4 || index == 5))
            {
                switch (index)
                {
                    case 2: usedIndex = 4; break;
                    case 3: usedIndex = 5; break;
                    case 4: usedIndex = 2; break;
                    case 5: usedIndex = 3; break;
                }
            }

            return (PadButtons)usedIndex;
        }

        /// <summary>
        /// Action when a button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Pad_Click(object sender, MouseClickEntityEventArgs e)
        {
            YnSprite button = sender as YnSprite;

            if (button != null)
            {
                PadButtons direction = GetDirection(button.Name);
                VirtualPadPressedEventArgs vpEvent = new VirtualPadPressedEventArgs(direction);

                OnPressed(vpEvent);
            }
        }

        /// <summary>
        /// Action when a button is released
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Pad_Released(object sender, MouseReleaseEntityEventArgs e)
        {
            YnSprite button = sender as YnSprite;

            if (button != null)
            {
                PadButtons direction = GetDirection(button.Name);
                VirtualPadPressedEventArgs vpEvent = new VirtualPadPressedEventArgs(direction);

                OnReleased(vpEvent);
            }
        }
    }
}
