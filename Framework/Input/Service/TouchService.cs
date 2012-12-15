using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Yna.Framework.Helpers;

namespace Yna.Framework.Input.Service
{
    public class TouchService : GameComponent, ITouchService
    {
        private TouchCollection touchCollection;
        private TouchCollection lastTouchCollection;

        protected Vector2 [] _position;
        protected Vector2 [] _lastPosition;
        protected bool [] _pressed;
        protected bool [] _moved;
        protected bool [] _released;
        protected float [] _pressure;

        public Vector2 [] Position;
        public Vector2 [] LastPosition;

        public int MaxFingerPoints
        {
            get { return TouchPanel.GetCapabilities().MaximumTouchCount; }
        }

        public bool Available
        {
            get { return TouchPanel.IsGestureAvailable; }
        }

        public TouchService(Game game)
            : base(game)
        {
            ServiceHelper.Add<ITouchService>(this);

            touchCollection = TouchPanel.GetState();
            lastTouchCollection = touchCollection;

            _position = new Vector2 [MaxFingerPoints];
            _lastPosition = new Vector2 [MaxFingerPoints];
            _pressed = new bool [MaxFingerPoints];
            _moved = new bool [MaxFingerPoints];
            _released = new bool [MaxFingerPoints];
            _pressure = new float [MaxFingerPoints];
        }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < MaxFingerPoints; i++)
            {
                _position [i] = Vector2.Zero;
                _lastPosition [i] = Vector2.Zero;
                _pressed [i] = false;
                _moved [i] = false;
                _released [i] = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            lastTouchCollection = touchCollection;
            touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                for (int i = 0, l = touchCollection.Count; i < l; i++)
                {
                    _lastPosition [i].X = _position [i].X;
                    _lastPosition [i].Y = _lastPosition [i].Y;

                    _position [i].X = touchCollection [i].Position.X;
                    _position [i].Y = touchCollection [i].Position.Y;

                    _pressed[i] = touchCollection[i].State == TouchLocationState.Pressed;
                    _moved [i] = touchCollection [i].State == TouchLocationState.Moved;
                    _released [i] = touchCollection [i].State == TouchLocationState.Released;
                    _pressure [i] = touchCollection [i].Pressure;
                }
            }
        }

        #region ITouchService Membres

        bool ITouchService.Pressed(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _pressed [id];
        }

        bool ITouchService.Released(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _released [id];
        }

        bool ITouchService.Moved(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _moved [id];
        }

        float ITouchService.GetPressure(int id)
        {
            if (id >= MaxFingerPoints)
                return 0.0f;

            return _pressure [id];
        }

        #endregion
    }
}
