using System;
using System.Collections.Generic;

namespace Yna.Utils
{
    /// <summary>
    /// An hybrid collection who work like a Queue with the advantages
    /// of a List. It's a List object with 3 extension methods who simulates a Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListQueue<T> : List<T>
    {
        /// <summary>
        /// Add an item at the end of the list
        /// </summary>
        /// <param name="item"></param>
        public void EnQueue(T item)
        {
            Add(item);
        }

        /// <summary>
        /// Get the first item and remove it from the list
        /// </summary>
        /// <returns>The first item of the list</returns>
        public T DeQueue()
        {
            if (this.Count == 0)
                throw new InvalidOperationException("The list is empty");

            T result = this[0];
            
            RemoveAt(0);

            return result;
        }

        /// <summary>
        /// Return the first item of the list without remove it
        /// </summary>
        /// <returns>The last item of the list</returns>
        public T Peek()
        {
            if (this.Count == 0)
                throw new InvalidOperationException("The list is empty");

            return this[0];
        }
    }
}
