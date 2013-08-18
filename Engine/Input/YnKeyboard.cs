// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input
{
    public class YnKeyboard : GameComponent
    {
        KeyboardState kbState;
        KeyboardState lastKbState;

        public YnKeyboard(Game game)
            : base(game)
        {
            kbState = Keyboard.GetState();
            lastKbState = kbState;
        }

        public override void Update(GameTime gameTime)
        {
            lastKbState = kbState;
            kbState = Keyboard.GetState();
            base.Update(gameTime);
        }

        public bool Pressed(Keys key)
        {
            return kbState.IsKeyDown(key);
        }

        public bool Released(Keys key)
        {
            return kbState.IsKeyUp(key);
        }

        public bool JustPressed(Keys key)
        {
            return kbState.IsKeyUp(key) && lastKbState.IsKeyDown(key);
        }

        public bool JustReleased(Keys key)
        {
            return kbState.IsKeyDown(key) && lastKbState.IsKeyUp(key);
        }

        public bool Up
        {
            get { return this.Pressed(Keys.Up); }
        }

        public bool Down
        {
            get { return this.Pressed(Keys.Down); }
        }

        public bool Left
        {
            get { return this.Pressed(Keys.Left); }
        }

        public bool Right
        {
            get { return this.Pressed(Keys.Right); }
        }

        public bool Enter
        {
            get { return this.Pressed(Keys.Enter); }
        }

        public bool Space
        {
            get { return this.Pressed(Keys.Space); }
        }

        public bool Escape
        {
            get { return this.Pressed(Keys.Escape); }
        }

        public bool LeftControl
        {
            get { return this.Pressed(Keys.LeftControl); }
        }

        public bool LeftShift
        {
            get { return this.Pressed(Keys.LeftShift); }
        }

        public bool Tab
        {
            get { return this.Pressed(Keys.Tab); }
        }
    }
}
