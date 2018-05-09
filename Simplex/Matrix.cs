namespace Simplex
{
    internal class Matrix<T>
    {
        #region Fields

        private readonly int rowCount;
        private readonly int columnCount;

        #endregion

        #region Constructors

        public Matrix(int rows, int columns)
        {
            rowCount = rows;
            columnCount = columns;

            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The full row based matrix.<para/>
        /// Key: matrix.Rows[row][column].
        /// </summary>
        public Range<T>[] Rows { get; private set; }

        /// <summary>
        /// The full column based matrix.<para/>
        /// Key: matrix.Columns[column][row].
        /// </summary>
        public Range<T>[] Columns { get; private set; }

        /// <summary>
        /// The matrix without the values column.<para/>
        /// Key: matrix.CoeffRows[row][column].
        /// </summary>
        public Range<T>[] CoeffRows { get; private set; }

        /// <summary>
        /// The matrix without the objective row.<para/>
        /// Key: matrix.ConstraintColumns[column][row].
        /// </summary>
        public Range<T>[] ConstraintColumns { get; private set; }

        #endregion

        #region Methods

        private void Initialize()
        {
            Rows = new Range<T>[rowCount];
            CoeffRows = new Range<T>[rowCount];
            for (int i = 0; i < rowCount; i++)
            {
                Rows[i] = new Range<T>(columnCount);
                CoeffRows[i] = new Range<T>(columnCount - 1, false);
                for (int j = 0; j < columnCount - 1; j++)
                {
                    CoeffRows[i].SetWrapper(j, Rows[i].GetWrapper(j));
                }
            }

            Columns = new Range<T>[columnCount];
            ConstraintColumns = new Range<T>[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                Columns[i] = new Range<T>(rowCount, false);
                ConstraintColumns[i] = new Range<T>(rowCount - 1, false);
                for (int j = 0; j < rowCount - 1; j++)
                {
                    Columns[i].SetWrapper(j, Rows[j].GetWrapper(i));
                    ConstraintColumns[i].SetWrapper(j, Rows[j].GetWrapper(i));
                }
                Columns[i].SetWrapper(rowCount - 1, Rows[rowCount - 1].GetWrapper(i));
            }
        }

        #endregion
    }    
}
