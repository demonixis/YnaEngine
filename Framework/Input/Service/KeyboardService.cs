using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Yna;
using Yna.Helpers;

namespace Yna.Input.Service
{
    public class KeyboardService : GameComponent, IKeyboardService
    {
        KeyboardState kbState;
        KeyboardState lastKbState;

        public KeyboardService(Game game)
            : base(game)
        {
            ServiceHelper.Add<IKeyboardService>(this);
            kbState = Keyboard.GetState();
            lastKbState = kbState;
        }

        public override void Update(GameTime gameTime)
        {
            lastKbState = kbState;
            kbState = Keyboard.GetState();
            base.Update(gameTime);
        }

        #region IKeyboardService Membres

        bool IKeyboardService.Pressed(Keys key)
        {
            return kbState.IsKeyDown(key);
        }

        bool IKeyboardService.Released(Keys key)
        {
            return kbState.IsKeyUp(key);
        }

        bool IKeyboardService.JustPressed(Keys key)
        {
            return kbState.IsKeyUp(key) && lastKbState.IsKeyDown(key);
        }
		
        bool IKeyboardService.JustReleased(Keys key)
        {
            return kbState.IsKeyDown(key) && lastKbState.IsKeyUp(key);
        }
		#endregion
    }
}
