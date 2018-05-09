using System.Collections;
using System.Collections.Generic;

namespace Simplex
{
    /// <summary>
    /// A list structure containing wrapped values.
    /// </summary>
    internal class Range<T> : IEnumerable<T>
    {
        #region Fields

        private Wrapper<T>[] values;
        private bool[] set;

        #endregion

        #region Constructors

        public Range(int length, bool initialize = true)
        {
            values = new Wrapper<T>[length];
            set = new bool[length];
            Length = length;

            if (initialize)
            {
                for (int i = 0; i < length; i++)
                {
                    values[i] = new Wrapper<T>();
                    set[i] = true;
                }
            }
        }

        #endregion

        #region Properties

        public int Length { get; private set; }

        #endregion

        #region Methods

        public T this[int index]
        {
            get => values[index].Value;
            set => values[index].Value = value;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Length; i++)
            {
                if (values[i].Value.Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        internal Wrapper<T> GetWrapper(int index)
        {
            return values[index];
        }

        internal void SetWrapper(int index, Wrapper<T> wrapper)
        {
            if (!set[index])
            {
                values[index] = wrapper;
                set[index] = true;
            }
        }

        #endregion

        #region IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in values)
            {
                yield return value.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
