using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Yna.Framework;
using Yna.Framework.Display;
using Yna.Framework.Display.Event;
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

        public YnVirtualPad(string[] textures)
        {
            InitializeDefault();
            InitializeWithTextures(textures);
            InitializeButtons();
        }

        private void InitializeWithoutTextures()
        {
            Color normal = new Color(15, 21, 25);
            Color strafe = new Color(68, 89, 100);
            Color pause = new Color(1, 11, 111);

            _upPad = new YnSprite(_inputRectangle, normal);
            _downPad = new YnSprite(_inputRectangle, normal);
            _leftPad = new YnSprite(_inputRectangle, strafe);
            _rightPad = new YnSprite(_inputRectangle, strafe);

            _strafeLeftPad = new YnSprite(_inputRectangle, normal);
            _strafeRightPad = new YnSprite(_inputRectangle, normal);

            _buttonActionA = new YnSprite(_inputRectangle, Color.Red);
            _buttonActionB = new YnSprite(_inputRectangle, Color.Blue);
            _buttonPause = new YnSprite(_inputRectangle, new Color(22, 110, 130));
        }

        private void InitializeDefault()
        {
            _margin = new Vector2(3, 3);
            _padding = new Vector2(5, 5);
            _inputRectangle = new Rectangle(0, 0, 65, 65);
            _inverseDirectionStrafe = false;
        }

        private void InitializeWithTextures(string[] textures)
        {
            if (textures.Length < 9)
            {
                InitializeButtons();
            }
            else
            {
                _upPad = new YnSprite(textures[0]);
                _downPad = new YnSprite(textures[1]);
                _leftPad = new YnSprite(textures[2]);
                _rightPad = new YnSprite(textures[3]);

                _strafeLeftPad = new YnSprite(textures[4]);
                _strafeRightPad = new YnSprite(textures[5]);

                _buttonActionA = new YnSprite(textures[6]);
                _buttonActionB = new YnSprite(textures[7]);
                _buttonPause = new YnSprite(textures[8]);
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
                sprite.Click += new EventHandler<MouseClickSpriteEventArgs>(Pad_Click);
                sprite.JustClicked += new EventHandler<MouseClickSpriteEventArgs>(Pad_Click);
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

            _strafeLeftPad.Position = new Vector2(X + _padding.X, Y + _leftPad.Height + _margin.Y);
            _downPad.Position = new Vector2(_strafeLeftPad.X + _strafeLeftPad.Width + _margin.X, _strafeLeftPad.Y);
            _strafeRightPad.Position = new Vector2(_downPad.X + _downPad.Width + _margin.X, _strafeLeftPad.Y);

            _buttonActionA.Position = new Vector2(YnG.DeviceWidth - (3 * _inputRectangle.Width), YnG.DeviceHeight - _inputRectangle.Height);
            _buttonActionB.Position = new Vector2(_buttonActionA.X + 10 + _buttonActionA.Width, _buttonActionA.Y);
            _buttonPause.Position = new Vector2(10 + _margin.X, 10 + _margin.Y);
        }


        protected virtual void Pad_Click(object sender, MouseClickSpriteEventArgs e)
        {
            YnSprite button = sender as YnSprite;

            if (button != null)
            {
                string[] temp = button.Name.Split(new char[] { '_' });

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

                ControlDirection direction = (ControlDirection)usedIndex;
                VirtualPadPressedEventArgs vpEvent = new VirtualPadPressedEventArgs(direction);

                if (e.JustClicked)
                    OnJustPressed(vpEvent);
                else
                    OnPressed(vpEvent);
            }
        }
    }
}
