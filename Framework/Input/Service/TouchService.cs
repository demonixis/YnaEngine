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
        private Vector2[] _position;
        private Vector2[] _lastPosition;
        private Vector2[] _direction;
        private Vector2[] _lastDirection;
        private bool[] _pressed;
        private bool[] _moved;
        private bool[] _released;
        private float[] _pressure;
        private int _maxFingerPoints;
        private bool _needUpdate;


        public int MaxFingerPoints
        {
            get { return _maxFingerPoints; }
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
#if MONOGAME && (WINDOWS || LINUX)
            _maxFingerPoints = 0;
#else
            if (TouchPanel.GetCapabilities().IsConnected)
                _maxFingerPoints = TouchPanel.GetCapabilities().MaximumTouchCount;
            else
                _maxFingerPoints = 0;
#endif
            _needUpdate = true;
        }

        public void SetMaxFingerPoints(int fingerCount)
        {
            if (fingerCount < 0)
                _maxFingerPoints = 0;
            else
            {
                if (fingerCount > TouchPanel.GetCapabilities().MaximumTouchCount)
                    _maxFingerPoints = TouchPanel.GetCapabilities().MaximumTouchCount;
                else
                    _maxFingerPoints = fingerCount;
            }

            _needUpdate = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            _position = new Vector2[MaxFingerPoints];
            _lastPosition = new Vector2[MaxFingerPoints];
            _direction = new Vector2[MaxFingerPoints];
            _lastDirection = new Vector2[MaxFingerPoints];

            _pressed = new bool[MaxFingerPoints];
            _moved = new bool[MaxFingerPoints];
            _released = new bool[MaxFingerPoints];
            _pressure = new float[MaxFingerPoints];

            for (int i = 0; i < MaxFingerPoints; i++)
            {
                _position[i] = Vector2.Zero;
                _lastPosition[i] = Vector2.Zero;
                _direction[i] = Vector2.Zero;
                _lastDirection[i] = Vector2.Zero;
                _pressed[i] = false;
                _moved[i] = false;
                _released[i] = false;
                _pressure[i] = 0.0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_needUpdate)
            {
                Initialize();
                _needUpdate = false;
            }

            lastTouchCollection = touchCollection;
            touchCollection = TouchPanel.GetState();
            
            if (MaxFingerPoints > 0)
            {
                if (touchCollection.Count > 0)
                {
                    int touchCount = touchCollection.Count;

                    for (int i = 0; i < MaxFingerPoints; i++)
                    {
                        if (i < touchCount)
                        {
                            UpdateTouchState(i);
                        }
                        else
                        {
                            RestoreTouchState(i);
                        }
                    }
                }
            }
        }

        private void UpdateTouchState(int index)
        {
            _lastPosition[index].X = _position[index].X;
            _lastPosition[index].Y = _position[index].Y;

            _lastDirection[index].X = _direction[index].X;
            _lastDirection[index].Y = _direction[index].Y;

            _position[index].X = touchCollection[index].Position.X;
            _position[index].Y = touchCollection[index].Position.Y;

            _direction[index].X = _position[index].X - _lastPosition[index].X;
            _direction[index].Y = _position[index].X - _lastPosition[index].Y;

            _pressed[index] = touchCollection[index].State == TouchLocationState.Pressed;
            _moved[index] = touchCollection[index].State == TouchLocationState.Moved;
            _released[index] = touchCollection[index].State == TouchLocationState.Released;

#if WINDOWS_PHONE_7
            _pressure[index] = touchCollection[index].Pressure;
#else
            _pressure[index] = 1.0f;
#endif
        }

        private void RestoreTouchState(int index)
        {
            _lastPosition[index].X = _position[index].X;
            _lastPosition[index].Y = _position[index].Y;

            _lastDirection[index].X = _direction[index].X;
            _lastDirection[index].Y = _direction[index].Y;

            _position[index].X = 0;
            _position[index].Y = 0;

            _direction[index].X = _position[index].X - _lastPosition[index].X;
            _direction[index].Y = _position[index].X - _lastPosition[index].Y;

            _pressed[index] = false;
            _moved[index] = false;
            _released[index] = false;

            _pressure[index] = 0.0f;
        }

        #region ITouchService Membres

        bool ITouchService.Pressed(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _pressed[id];
        }

        bool ITouchService.Released(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _released[id];
        }

        bool ITouchService.Moved(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _moved[id];
        }

        Vector2 ITouchService.GetPosition(int id)
        {
            if (id >= MaxFingerPoints)
                return Vector2.Zero;

            return _position[id];
        }

        Vector2 ITouchService.GetLastPosition(int id)
        {
            if (id >= MaxFingerPoints)
                return Vector2.Zero;

            return _lastPosition[id];
        }

        Vector2 ITouchService.GetDirection(int id)
        {
            if (id >= MaxFingerPoints)
                return Vector2.Zero;

            return _direction[id];
        }

        Vector2 ITouchService.GetLastDirection(int id)
        {
            if (id >= MaxFingerPoints)
                return Vector2.Zero;

            return _lastDirection[id];
        }

        bool ITouchService.JustPressed(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return (lastTouchCollection[id].State == TouchLocationState.Pressed || lastTouchCollection[id].State == TouchLocationState.Moved) && touchCollection[id].State == TouchLocationState.Released;
        }

        bool ITouchService.JustReleased(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return touchCollection[id].State == TouchLocationState.Released && (lastTouchCollection[id].State == TouchLocationState.Pressed || lastTouchCollection[id].State == TouchLocationState.Moved);
        }

        bool ITouchService.Moving(int id)
        {
            if (id >= MaxFingerPoints)
                return false;

            return _position[id] != _lastPosition[id];
        }

        float ITouchService.GetPressureLevel(int id)
        {
            if (id >= MaxFingerPoints)
                return 0.0f;

            return _pressure[id];
        }

        #endregion
    }
}
