using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Yna.Engine.Input.Service
{
    public class KeyboardComponent : GameComponent
    {
        KeyboardState kbState;
        KeyboardState lastKbState;

        public KeyboardComponent(Game game)
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
    }
}
