// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Yna.Engine.Graphics3D.Camera
{
    public class CameraChangedEventArgs : EventArgs
    {
        public BaseCamera ActiveCamera;

        public CameraChangedEventArgs()
        {
            ActiveCamera = null;
        }

        public CameraChangedEventArgs(BaseCamera camera)
        {
            ActiveCamera = camera;
        }
    }

    /// <summary>
    /// A camera manager
    /// </summary>
    public sealed class CameraManager : ICollection<BaseCamera>
    {
        private BaseCamera[] _cameras;
        private int _arraySize;
        private int _activeCameraIndex;

        /// <summary>
        /// Gets the number of cameras.
        /// </summary>
        public int Count
        {
            get { return _arraySize; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public event EventHandler<CameraChangedEventArgs> ActiveCameraChanged = null;

        private void OnActiveCameraChanged(CameraChangedEventArgs e)
        {
            if (ActiveCameraChanged != null)
                ActiveCameraChanged(this, e);
        }

        #region Constructors

        /// <summary>
        /// Create a default camera manager without cameras.
        /// </summary>
        /// <param name="camera">A camera</param>
        public CameraManager()
        {
            _cameras = null;
            _activeCameraIndex = -1;
            _arraySize = 0;
        }

        /// <summary>
        /// Create a camera manager with a default camera
        /// </summary>
        /// <param name="camera">A camera</param>
        public CameraManager(BaseCamera camera)
        {
            _cameras = new BaseCamera[1];
            _cameras[0] = camera;
            _activeCameraIndex = 0;
            _arraySize = 1;
        }

        /// <summary>
        /// Create a camera manager with cameras. The latest camera is the default camera
        /// </summary>
        /// <param name="cameras">Array of cameras</param>
        public CameraManager(BaseCamera[] cameras)
        {
            _arraySize = cameras.Length;
            _cameras = new BaseCamera[_arraySize];
            
            for (int i = 0; i < _arraySize; i++)
                _cameras[i] = cameras[i];

            _activeCameraIndex = _arraySize - 1;
        }

        #endregion

        /// <summary>
        /// Update the active camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (_arraySize > 0)
                _cameras[_activeCameraIndex].Update(gameTime);
        }

        /// <summary>
        /// Gets active camera.
        /// </summary>
        /// <returns>Return the active camera.</returns>
        public BaseCamera GetActiveCamera()
        {
            return _arraySize > 0 ? _cameras[_activeCameraIndex] : null;
        }

        /// <summary>
        /// Set active camera.
        /// </summary>
        /// <param name="index">Index of camera to use</param>
        public void SetActiveCamera(int index)
        {
            if (index > -1 && index < _arraySize)
            {
                _activeCameraIndex = index;
                OnActiveCameraChanged(new CameraChangedEventArgs(_cameras[_activeCameraIndex]));
            }
        }

        /// <summary>
        /// Set active camera. If the camera is not added to the manager, it added and selected to be the active camera.
        /// </summary>
        /// <param name="camera"></param>
        public void SetActiveCamera(BaseCamera camera)
        {
            bool founded = false;
            int i = 0;

            while (i < _arraySize && !founded)
            {
                if (_cameras[i] == camera)
                {
                    SetActiveCamera(i);
                    founded = true;
                }
                i++;
            }

            if (!founded)
            {
                Add(camera);
                SetActiveCamera(_arraySize - 1);
            }
        }

        /// <summary>
        /// Sets the next camera active.
        /// </summary>
        /// <returns>The active camera index.</returns>
        public int NextCamera()
        {
            if (_activeCameraIndex < _arraySize - 1)
                _activeCameraIndex++;
            else
                _activeCameraIndex = 0;

            OnActiveCameraChanged(new CameraChangedEventArgs(_cameras[_activeCameraIndex]));

            return _activeCameraIndex;
        }

        /// <summary>
        /// Sets the previous camera active.
        /// </summary>
        /// <returns>The active camera index.</returns>
        public int PreviousCamera()
        {
            if (_activeCameraIndex > 0)
                _activeCameraIndex--;
            else
                _activeCameraIndex = _arraySize - 1;
            
            OnActiveCameraChanged(new CameraChangedEventArgs(_cameras[_activeCameraIndex]));

            return _activeCameraIndex;
        }

        /// <summary>
        /// Add a new camera
        /// </summary>
        /// <param name="item">A camera</param>
        public void Add(BaseCamera item)
        {
            bool alreadyAddded = false;
            int i = 0;

            while (i < _arraySize && !alreadyAddded)
            {
                if (_cameras[i] == item)
                    alreadyAddded = true;
                i++;
            }

            if (!alreadyAddded)
            {
                BaseCamera[] temp = new BaseCamera[_arraySize + 1];
                CopyTo(temp, 0);
                temp[_arraySize] = item;
                _arraySize++;

                _cameras = temp;

                if (_activeCameraIndex < 0)
                    _activeCameraIndex = 0;
            }
        }

        /// <summary>
        /// Remove all camera of the manager.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _arraySize; i++)
            {
                if (_cameras[i] != null)
                    _cameras[i] = null;
            }
            _arraySize = 0;
            _activeCameraIndex = -1;
            _cameras = null;
        }

        /// <summary>
        /// Determine if the collection contains the camera.
        /// </summary>
        /// <param name="item">Camera to test.</param>
        /// <returns>Return true if the camera is in the collection, otherwise return false.</returns>
        public bool Contains(BaseCamera item)
        {
            bool result = false;

            for (int i = 0, l = _cameras.Length; i < l; i++)
            {
                if (_cameras[i] == item)
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Copy the collection in an array.
        /// </summary>
        /// <param name="array">Blank array</param>
        /// <param name="startIndex">Start index.</param>
        public void CopyTo(BaseCamera[] array, int startIndex)
        {
            int max = Math.Min(startIndex + _arraySize, array.Length);
            int camIndex = 0;

            for (int i = startIndex; i < max; i++)
                array[i] = _cameras[camIndex++];
        }

        /// <summary>
        /// Remove a camera from the collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(BaseCamera item)
        {
            int arraySize = _cameras.Length;
            int removedIndex = -1;

            for (int i = 0, l = arraySize; i < l; i++)
            {
                if (_cameras[i] == item)
                {
                    _cameras[i] = null;
                    removedIndex = i;
                }
            }

            if (removedIndex > -1)
            {
                BaseCamera[] temp = new BaseCamera[arraySize - 1];

                for (int i = 0; i < arraySize - 1; i++)
                {
                    if (i >= removedIndex)
                        temp[i] = _cameras[i + 1];
                    else
                        temp[i] = _cameras[i];
                }

                _cameras = temp;
                _arraySize -= 1;

                if (_activeCameraIndex > 0 && _activeCameraIndex == removedIndex)
                {
                    _activeCameraIndex--;
                    OnActiveCameraChanged(new CameraChangedEventArgs(_cameras[_activeCameraIndex]));
                }

                return true;
            }

            return false;
        }

        public IEnumerator<BaseCamera> GetEnumerator()
        {
            foreach (BaseCamera camera in _cameras)
                yield return camera;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
