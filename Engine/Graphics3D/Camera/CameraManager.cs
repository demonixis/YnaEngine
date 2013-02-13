using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics3D.Camera
{
    public class CameraManager : ICollection<BaseCamera>
    {
        private BaseCamera [] _cameras;
        private int _arraySize;
        private int _activeCameraIndex;

        public CameraManager(BaseCamera camera)
        {
            _cameras = new BaseCamera[1];
            _activeCameraIndex = 0;
            _arraySize = 1;
            Add(camera);
        }

        public CameraManager(BaseCamera[] cameras)
        {
            _arraySize = cameras.Length;
            _cameras = new BaseCamera[_arraySize];

            for (int i = 0; i < _arraySize; i++)
                _cameras[i] = cameras[i];

            _activeCameraIndex = _arraySize - 1;
        }

        public BaseCamera GetActiveCamera()
        {
            return _cameras[_activeCameraIndex];
        }

        public void SetActiveCamera(int index)
        {
            if (index > -1 && index < _arraySize)
                _activeCameraIndex = index;
        }

        /// <summary>
        /// Sets the next camera active
        /// </summary>
        /// <returns></returns>
        public bool NextCamera()
        {
            if (_activeCameraIndex < _arraySize - 1)
            {
                _activeCameraIndex++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the previous camera active
        /// </summary>
        /// <returns></returns>
        public bool PreviousCamera()
        {
            if (_activeCameraIndex > 0)
            {
                _activeCameraIndex--;
                return true;
            }

            return false;
        }

        public void Add(BaseCamera item)
        {
            bool canAdd = true;
            int i = 0;

            while (i < _arraySize && canAdd)
            {
                if (_cameras[i] == item)
                    canAdd = false;
                i++;
            }

            if (canAdd)
            {
                BaseCamera[] temp = new BaseCamera[_arraySize + 1];
                CopyTo(temp, 0);
                temp[_arraySize] = item;
                _arraySize++;
            }
        }

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

        public void CopyTo(BaseCamera[] array, int arrayIndex)
        {
            for (int i = 0; i < _arraySize; i++)
                array[i] = _cameras[i];
        }

        public int Count
        {
            get { return _arraySize; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

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
            throw new NotImplementedException();
        }
    }
}
