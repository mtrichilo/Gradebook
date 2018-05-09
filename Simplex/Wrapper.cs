namespace Simplex
{
    /// <summary>
    /// A wrapper for value types.
    /// </summary>
    internal class Wrapper<T>
    {
        #region Constructors

        public Wrapper() { }

        public Wrapper(T value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public T Value { get; set; }

        #endregion
    }
}
