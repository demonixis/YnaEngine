using Microsoft.Xna.Framework;
using Yna.Engine.Input.Service;

namespace Yna.Engine.Input
{
    /// <summary>
    /// The touch manager
    /// </summary>
    public class YnTouch
    {
        private TouchComponent _touchComponent;

        /// <summary>
        /// Check if the first finger id has moved
        /// </summary>
        public bool Moved
        {
            get { return _touchComponent.Moved(0); }
        }

        /// <summary>
        /// Check if the first finger id has tapped
        /// </summary>
        public bool Tapped
        {
            get { return _touchComponent.Pressed(0); }
        }

        /// <summary>
        /// Check if the first finger id has released
        /// </summary>
        public bool Released
        {
            get { return _touchComponent.Released(0); }
        }

        /// <summary>
        /// Gets the position of the first finger 
        /// </summary>
        public Vector2 Position
        {
            get { return _touchComponent.GetPosition(0); }
        }

        /// <summary>
        /// Gets the last position of the first finger 
        /// </summary>
        public Vector2 LastPosition
        {
            get { return _touchComponent.GetLastPosition(0); }
        }

        /// <summary>
        /// Gets the delta of the first finger 
        /// </summary>
        public Vector2 Delta
        {
            get 
            { 
                Vector2 delta = _touchComponent.GetPosition(0) - _touchComponent.GetLastPosition(0);
                return delta;
            }
        }

        /// <summary>
        /// Create a new helper for touch input
        /// </summary>
        /// <param name="component">An instance of TouchComponent</param>
        public YnTouch(TouchComponent component)
        {
            _touchComponent = component;
        }

        public Vector2 GetPosition(int id)
        {
            return _touchComponent.GetPosition(id);
        }

        public Vector2 GetLastPosition(int id)
        {
            return _touchComponent.GetLastPosition(id);
        }

        public Vector2 GetDirection(int id)
        {
            return _touchComponent.GetDirection(id);
        }

        public Vector2 GetLastDirection(int id)
        {
            return _touchComponent.GetLastDirection(id);
        }

        public bool JustPressed(int id)
        {
            return _touchComponent.JustPressed(id);
        }

        public bool JustReleased(int id)
        {
            return _touchComponent.JustReleased(id);
        }

        public bool Moving(int id)
        {
            return _touchComponent.Moving(id);
        }

        public float GetPressureLevel(int id)
        {
            return _touchComponent.GetPressureLevel(id);
        }
    }
}
