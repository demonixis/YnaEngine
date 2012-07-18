using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Yna.Utils
{
    public class CircularArray : IEnumerable
    {
        #region Attributs
        private int[] array;
        private int indice;
        #endregion

        #region Propriétés
        public int[] Array
        {
            get { return array; }
            set { array = value; }
        }

        public int Indice
        {
            get { return indice; }
            set { indice = value; }
        }
        #endregion

        #region Constructeurs
        public CircularArray()
        {
            array = new int[1];
            array[0] = new int();
            indice = 0;
        }

        public CircularArray(int size)
        {
            array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = new int();
            indice = 0;
        }
        #endregion

        public int Next()
        {
            indice++;
            if (indice >= array.Length)
                indice = 0;
            return indice;
        }

        public int Previous()
        {
            if (indice <= 0)
                indice = array.Length - 1;
            else
                indice--;
            return indice;
        }

        #region IEnumerable Membres

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < array.Length; i++)
                yield return array[i];
        }

        #endregion
    }
}
