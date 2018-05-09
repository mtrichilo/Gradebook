using System.Collections;
using System.Collections.Generic;

namespace Simplex
{
    /// <summary>
    /// Represents a list that allows the insertion of elements at any index
    /// without an <see cref="ArgumentOutOfRangeException"/> being thrown.
    /// </summary>
    /// <typeparam name="T">The type of elements in this list.</typeparam>
    public class DefaultList<T> : IList<T>
    {
        private IList<T> list = new List<T>();

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        public T this[int index]
        {
            get => list[index];
            set
            {
                if (index >= Count)
                {
                    Insert(index, value);
                }
                else
                {
                    list[index] = value;
                }
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the list.
        /// </summary>
        public int Count => list.Count;

        /// <summary>
        /// Returns whether the list is read-only.
        /// </summary>
        public bool IsReadOnly => list.IsReadOnly;

        /// <summary>
        /// Adds an element to the end of the list.
        /// </summary>
        /// <param name="item">The element to be added to the end of the list.</param>
        public void Add(T item)
        {
            list.Add(item);
        }

        /// <summary>
        /// Removes all elements from the list.
        /// </summary>
        public void Clear()
        {
            list.Clear();
        }

        /// <summary>
        /// Determines whether an element is in the list.
        /// </summary>
        /// <param name="item">The element to locate in the list.</param>
        /// <returns>True if the item is found in the list.</returns>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Copies the entire list to an array, starting at the specified index of the
        /// target array.
        /// </summary>
        /// <param name="array">The array that is the destination of the elements
        /// copied from this list.</param>
        /// <param name="arrayIndex">The zero-based index in the given array at which
        /// copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        /// Determines the specific index of the given element.
        /// </summary>
        /// <param name="item">The element to locate in the list.</param>
        /// <returns>The zero-based index of the first occurance of the item within the list.</returns>
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Inserts the element at the specified index. If the given index is greater
        /// than Count, the default value of T will be added to the list from this[Count]
        /// to this[index - 1]. 
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        public void Insert(int index, T item)
        {
            if (index > Count)
            {
                for (int i = Count; i < index; i++)
                {
                    Add(default(T));
                }
            }
            list.Insert(index, item);
        }

        /// <summary>
        /// Removes the first occurance of the element from the list.
        /// </summary>
        /// <param name="item">The element to remove from the list.</param>
        /// <returns>True if the item was successfully removed.</returns>
        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        /// <summary>
        /// Removes the element at the specified index of the list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
