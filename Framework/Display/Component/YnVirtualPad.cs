﻿using System;
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
        Up = 0, Down, Left, Right, StrafeLeft, StrafeRight, None
    }


    public class YnVirtualPad : YnGroup
    {
        private Vector2 _margin;
        private YnSprite _upPad;
        private YnSprite _downPad;
        private YnSprite _leftPad;
        private YnSprite _rightPad;
        private YnSprite _strafeLeftPad;
        private YnSprite _strafeRightPad;
        private Rectangle _rectangle;
   
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
            _margin = new Vector2(3, 2);
            _rectangle = new Rectangle(0, 0, 65, 65);

            InitializeWithoutTextures();

            InitializeDefault();
        }

        public YnVirtualPad(string [] textures)
        {
            _margin = new Vector2(3, 2);
            _rectangle = new Rectangle(0, 0, 65, 65);

            InitializeWithTextures(textures);

            InitializeDefault();
        }

        private void InitializeWithoutTextures()
        {
            Color normal = new Color(15, 21, 25);
            Color strafe = new Color(68, 89, 100);

            _upPad = new YnSprite(_rectangle, normal);
            _downPad = new YnSprite(_rectangle, normal);
            _leftPad = new YnSprite(_rectangle, normal);
            _rightPad = new YnSprite(_rectangle, normal);

            _strafeLeftPad = new YnSprite(_rectangle, strafe);
            _strafeRightPad = new YnSprite(_rectangle, strafe);
        }

        private void InitializeWithTextures(string[] textures)
        {
            if (textures.Length < 6)
            {
                InitializeDefault();
            }
            else
            {
                _upPad = new YnSprite(textures[0]);
                _downPad = new YnSprite(textures[1]);
                _leftPad = new YnSprite(textures[2]);
                _rightPad = new YnSprite(textures[3]);

                _strafeLeftPad = new YnSprite(textures[4]);
                _strafeRightPad = new YnSprite(textures[5]);
            }
        }

        private void InitializeDefault()
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

            UpdateLayoutPosition();

            Width = (int)(_leftPad.Width + _upPad.Width + _rightPad.Width + 2 * _margin.X);
            Height = (int)(_leftPad.Height + _strafeLeftPad.Height + _margin.Y);
        }

        public void UpdateLayoutPosition()
        {
            _leftPad.Position = new Vector2(X, Y);
            _upPad.Position = new Vector2(_leftPad.X + _leftPad.Width + _margin.X, _leftPad.Y);
            _rightPad.Position = new Vector2(_upPad.X + _upPad.Width + _margin.X, _leftPad.Y);

            _strafeLeftPad.Position = new Vector2(X, Y + _leftPad.Height + _margin.Y);
            _downPad.Position = new Vector2(_strafeLeftPad.X + _strafeLeftPad.Width + _margin.X, _strafeLeftPad.Y);
            _strafeRightPad.Position = new Vector2(_downPad.X + _downPad.Width + _margin.X, _strafeLeftPad.Y);
        }

        public void UpdateScale(float scale)
        {
            foreach (YnEntity sceneObject in this)
                sceneObject.Scale = new Vector2(scale);
        }

        private void Pad_Click(object sender, MouseClickSpriteEventArgs e)
        {
            YnSprite button = sender as YnSprite;

            if (button != null)
            {
                string[] temp = button.Name.Split(new char[] { '_' });
                int index = int.Parse(temp[1].ToString());

                ControlDirection direction = (ControlDirection)index;
                VirtualPadPressedEventArgs vpEvent = new VirtualPadPressedEventArgs(direction);

                if (e.JustClicked)
                    OnJustPressed(vpEvent);
                else
                    OnPressed(vpEvent);
            }
        }
    }
}
