// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input
{
    public class YnKeyboard : GameComponent
    {
        KeyboardState _kbState;
        KeyboardState _lastKbState;

        public YnKeyboard(Game game)
            : base(game)
        {
            _kbState = Keyboard.GetState();
            _lastKbState = _kbState;
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            _lastKbState = _kbState;
            _kbState = Keyboard.GetState();
            base.Update(gameTime);
        }

        public bool Pressed(Keys key) => _kbState.IsKeyDown(key);
        public bool Released(Keys key) => _kbState.IsKeyUp(key);
        public bool JustPressed(Keys key) => _kbState.IsKeyUp(key) && _lastKbState.IsKeyDown(key);
        public bool JustReleased(Keys key) => _kbState.IsKeyDown(key) && _lastKbState.IsKeyUp(key);


        public bool Up => Pressed(Keys.Up);
        public bool Down => Pressed(Keys.Down);
        public bool Left => Pressed(Keys.Left);
        public bool Right => Pressed(Keys.Right);
        public bool Enter => Pressed(Keys.Enter);
        public bool Space => Pressed(Keys.Space);
        public bool Escape => Pressed(Keys.Escape);
        public bool LeftControl => Pressed(Keys.LeftControl);
        public bool LeftShift => Pressed(Keys.LeftShift);
        public bool Tab => Pressed(Keys.Tab);
    }
}
