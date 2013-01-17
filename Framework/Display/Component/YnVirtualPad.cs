using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Framework;
using Yna.Framework.Display;
using Yna.Framework.Display.Event;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Yna.Framework.Input;

namespace Yna.Framework.Display.Component
{
    public enum VirtualPadSize
    {
        Small = 0, Normal, Big
    }

    public enum ControlDirection
    {
        Up = 0, Down, Left, Right, StrafeLeft, StrafeRight, ButtonA, ButtonB, Pause, None
    }

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
        private int _buttonSpace;

        // Enable or disable buttons
        private bool _enableStrafeButtons;
        private bool _enableButtonA;
        private bool _enableButtonB;
        private bool _enableButtonPause;
        private bool _inverseDirectionStrafe;

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
            get { return _buttonSpace; }
            set
            {
                _buttonSpace = value;

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

        public event EventHandler<VirtualPadPressedEventArgs> Pressed = null;

        public event EventHandler<VirtualPadPressedEventArgs> JustPressed = null;

        public void OnPressed(VirtualPadPressedEventArgs e)
        {
            if (Pressed != null)
                Pressed(this, e);
        }

        public void OnJustPressed(VirtualPadPressedEventArgs e)
        {
            if (JustPressed != null)
                JustPressed(this, e);
        }

        #endregion

        public YnVirtualPad()
        {
            InitializeDefault();
            InitializeWithoutTextures();
            InitializeButtons();
        }

        private void InitializeWithoutTextures()
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

        private void InitializeDefault()
        {
            _margin = new Vector2(3, 3);
            _padding = new Vector2(5, 5);
            _inputRectangle = new Rectangle(0, 0, 65, 65);
            _inverseDirectionStrafe = false;
            _buttonSpace = 45;
        }

        /// <summary>
        /// Sets textures for the digital pad
        /// The array of Texture2D must contains 6 textures
        /// Order : up, down, left, right, strafeLeft, strafeRight
        /// </summary>
        /// <param name="textures"></param>
        private void SetDPadTextures(Texture2D[] textures)
        {
            if (textures.Length == 6)
            {
                _upPad.Texture = textures[0];
                _downPad.Texture = textures[1];
                _leftPad.Texture = textures[2];
                _rightPad.Texture = textures[3];

                _strafeLeftPad.Texture = textures[4];
                _strafeRightPad.Texture = textures[5];

                if (_assetLoaded)
                    UpdateLayout();
            }
        }

        private void InitializeButtons()
        {
            _upPad.Name = "Button_" + ((int)ControlDirection.Up).ToString();
            Add(_upPad);

            _downPad.Name = "Button_" + ((int)ControlDirection.Down).ToString();
            Add(_downPad);

            _strafeLeftPad.Name = "Button_" + ((int)ControlDirection.StrafeLeft).ToString();
            Add(_strafeLeftPad);

            _strafeRightPad.Name = "Button_" + ((int)ControlDirection.StrafeRight).ToString();
            Add(_strafeRightPad);

            _leftPad.Name = "Button_" + ((int)ControlDirection.Left).ToString();
            Add(_leftPad);

            _rightPad.Name = "Button_" + ((int)ControlDirection.Right).ToString();
            Add(_rightPad);

            _buttonActionA.Name = "Button_" + ((int)ControlDirection.ButtonA).ToString();
            Add(_buttonActionA);

            _buttonActionB.Name = "Button_" + ((int)ControlDirection.ButtonB).ToString();
            Add(_buttonActionB);

            _buttonPause.Name = "Button_" + ((int)ControlDirection.Pause).ToString();
            Add(_buttonPause);

            X = (int)(10 + _margin.X);
            Y = (int)(YnG.DeviceHeight - (2 * _downPad.Height) - (2 * _margin.Y));

            _alpha = 0.75f;

            foreach (YnSprite sprite in this)
            {
                sprite.Click += Pad_Click;
                sprite.JustClicked += Pad_Click;
                sprite.Alpha = _alpha;
            }
        }

        public override void LoadContent()
        {
            base.LoadContent();

            UpdateLayout();

            Width = (int)(_leftPad.Width + _upPad.Width + _rightPad.Width + 2 * _margin.X);
            Height = (int)(_leftPad.Height + _strafeLeftPad.Height + _margin.Y);

            _assetLoaded = true;
        }

        public virtual void UpdateLayout()
        {
            _leftPad.Position = new Vector2(X + _padding.X, Y - _padding.Y);
            _upPad.Position = new Vector2(_leftPad.X + _leftPad.Width + _margin.X, _leftPad.Y);
            _rightPad.Position = new Vector2(_upPad.X + _upPad.Width + _margin.X, _leftPad.Y);

            _strafeLeftPad.Position = new Vector2(X + _padding.X, (_upPad.Y + _upPad.Height) + _margin.Y);
            _downPad.Position = new Vector2(_strafeLeftPad.X + _strafeLeftPad.Width + _margin.X, _strafeLeftPad.Y);
            _strafeRightPad.Position = new Vector2(_downPad.X + _downPad.Width + _margin.X, _strafeLeftPad.Y);

            _buttonActionA.Position = new Vector2(YnG.DeviceWidth - ((2 * _inputRectangle.Width) + _padding.X + _margin.X + _buttonSpace), _downPad.Y);
            _buttonActionB.Position = new Vector2(_buttonActionA.X + _buttonActionA.Width + _margin.X + _buttonSpace, _downPad.Y);
            _buttonPause.Position = new Vector2(_padding.X, _padding.Y);
        }

        private ControlDirection GetDirection(string buttonName)
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

            return (ControlDirection)usedIndex;
        }

        protected virtual void Pad_Click(object sender, MouseClickSpriteEventArgs e)
        {
            YnSprite button = sender as YnSprite;

            if (button != null)
            {
                ControlDirection direction = GetDirection(button.Name);
                VirtualPadPressedEventArgs vpEvent = new VirtualPadPressedEventArgs(direction);

                if (e.JustClicked)
                    OnJustPressed(vpEvent);
                else
                    OnPressed(vpEvent);
            }
        }
    }


    public class YnVirtualPadController
    {
        public bool Up { get; internal set; }
        public bool Down { get; internal set; }
        public bool Left { get; internal set; }
        public bool Right { get; internal set; }
        public bool StrafeLeft { get; internal set; }
        public bool StrafeRight { get; internal set; }
        public bool ButtonA { get; internal set; }
        public bool ButtonB { get; internal set; }
        public bool ButtonPause { get; internal set; }

        private YnVirtualPad _virtualPad;

        public YnVirtualPad VirtualPad
        {
            get { return _virtualPad; }
            set { _virtualPad = value; }
        }

        public YnVirtualPadController()
        {
            _virtualPad = new YnVirtualPad();
            _virtualPad.Pressed += _virtualPad_Pressed;
            _virtualPad.JustPressed += _virtualPad_Pressed;
        }


        void _virtualPad_Pressed(object sender, VirtualPadPressedEventArgs e)
        {
            Up = e.Direction == ControlDirection.Up;
            Down = e.Direction == ControlDirection.Down;
            Left = e.Direction == ControlDirection.Left;
            Right = e.Direction == ControlDirection.Right;
            StrafeLeft = e.Direction == ControlDirection.StrafeLeft;
            StrafeRight = e.Direction == ControlDirection.StrafeRight;
            ButtonA = e.Direction == ControlDirection.ButtonA;
            ButtonB = e.Direction == ControlDirection.ButtonB;
            ButtonPause = e.Direction == ControlDirection.Pause;
        }

        public void LoadContent()
        {
            _virtualPad.LoadContent();
            _virtualPad.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            _virtualPad.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _virtualPad.Draw(gameTime, spriteBatch);
        }

        public void DrawOnSingleBatch(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _virtualPad.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}
