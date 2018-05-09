using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplex
{
    /// <summary>
    /// Represents a linear programming problem that can be solved using the 
    /// Simplex method.
    /// </summary>
    public class SimplexSolver
    {
        #region Fields

        private Matrix<double> matrix;
        private int rows;
        private int columns;
        private int objRow;
        private int valuesColumn;

        private IList<object> variablesDict = new List<object>();

        private Optimization objOptimization;
        private IList<double> objCoefficients = new DefaultList<double>();

        private IList<IList<double>> consCoefficients = new List<IList<double>>();
        private IList<Relationship> consRelationships = new List<Relationship>();
        private IList<double> consValues = new List<double>();
        private int consCount = 0;

        #endregion

        #region Methods

        /// <summary>
        /// Sets the objective function for this linear problem.
        /// Variables are referenced by indexes from the given list of coefficients.
        /// </summary>
        /// <param name="optimization">Minimize or maximize the eqaution?</param>
        /// <param name="coefficients">The variables and coefficients of the equation.</param>
        public void SetObjective(Optimization optimization, IDictionary<object, double> coefficients)
        {
            objOptimization = optimization;

            foreach (var variable in coefficients.Keys)
            {
                if (!variablesDict.Contains(variable))
                {
                    variablesDict.Add(variable);
                }

                var index = variablesDict.IndexOf(variable);
                objCoefficients[index] = coefficients[variable];
            }
        }

        /// <summary>
        /// Adds a constraint equation to this linear problem.
        /// Variables are referenced by indexes from the given list of coefficients.
        /// </summary>
        /// <param name="coefficients">The variables and coefficients of the equation.</param>
        /// <param name="relationship">The type of equality or inequality.</param>
        /// <param name="value">The value of the equation.</param>
        public void AddConstraint(IDictionary<object, double> coefficients, Relationship relationship, double value)
        {
            consCoefficients.Insert(consCount, new DefaultList<double>());

            foreach (var variable in coefficients.Keys)
            {
                if (!variablesDict.Contains(variable))
                {
                    variablesDict.Add(variable);
                }

                var index = variablesDict.IndexOf(variable);
                consCoefficients[consCount][index] = coefficients[variable];
            }

            consRelationships.Insert(consCount, relationship);
            consValues.Insert(consCount, value);
            consCount++;
        }

        /// <summary>
        /// Initializes the matrix and solves the linear prorgramming problem
        /// using the Simplex method.
        /// </summary>
        /// <returns>Whether or not an optimal solution exists.</returns>
        public bool Solve(out IDictionary<object, double> solution)
        {
            InitializeMatrix();
            PrintMatrix();
            solution = new Dictionary<object, double>();

            // While the matrix isn't in the feasible region, try and make it feasible.
            while (!IsFeasible(out int row))
            {
                if (!MakeFeasible(row)) return false;
            }

            // While there are any negative values in the objective row, pivot.
            while (matrix.CoeffRows[objRow].Any(value => value < 0))
            {
                if (!SolveSimplex()) return false;
            }

            PrintMatrix();
            solution = GetSolution();
            return true;
        }

        /// <summary>
        /// Initializes the matrix using the given objective and constraint equations.
        /// </summary>
        private void InitializeMatrix()
        {
            // Negate the coefficients if this is a maximization problem.
            if (objOptimization == Optimization.Max)
            {
                for (int i = 0; i < objCoefficients.Count; i++)
                {
                    objCoefficients[i] *= -1;
                }
            }

            // Calculate the number of eqaulities since they have to be split up
            // into two equations.
            int equalities = consRelationships.Where(r => r == Relationship.Equal).Count();

            // Set up rows, columns and create the matrix.
            rows = consCount + equalities + 1;
            objRow = rows - 1;

            columns = variablesDict.Count + consCount + equalities + 2;
            valuesColumn = columns - 1;

            matrix = new Matrix<double>(rows, columns);

            // Keep track of the current constraint and slack/surplus variable. Set all 
            // of the constraint coefficients.
            int cons = 0;
            int slackVar = variablesDict.Count;
            for (int i = 0; i < objRow; i++)
            {
                matrix.Rows[i][valuesColumn] = consValues[cons];

                for (int j = 0; j < valuesColumn; j++)
                {
                    matrix.Rows[i][j] = j < consCoefficients[cons].Count ? consCoefficients[cons][j] : 0;
                }

                // Use the correct inequality value to set up slack variables. Do not 
                // move onto the next constraint if the current one is a strict equality.
                switch (consRelationships[cons])
                {
                    case Relationship.Equal:
                        matrix.Rows[i][slackVar] = Relationship.GreaterThanOrEqual.GetValue();
                        consRelationships[cons] = Relationship.LessThanOrEqual;
                        break;
                    case Relationship.GreaterThanOrEqual:
                        matrix.Rows[i][slackVar] = Relationship.GreaterThanOrEqual.GetValue();
                        cons++;
                        break;
                    case Relationship.LessThanOrEqual:
                        matrix.Rows[i][slackVar] = Relationship.LessThanOrEqual.GetValue();
                        cons++;
                        break;
                    default:
                        throw new Exception("Invalid relationship.");
                }

                // Keep track of what slack/surplus variable we're on.
                slackVar++;
            }

            // Set the objective coefficients in the bottom row.
            for (int i = 0; i < columns; i++)
            {
                matrix.Rows[objRow][i] = i < objCoefficients.Count ? objCoefficients[i] : 0;
            }
            matrix.Rows[objRow][columns - 2] = 1;
        }

        /// <summary>
        /// Checks if the current matrix has a feasible solution.
        /// </summary>
        /// <param name="row">The row with an infeasible variable.</param>
        /// <returns>Whether or not the matrix has a feasible solution.</returns>
        private bool IsFeasible(out int row)
        {
            row = -1;
            for (int i = 0; i < valuesColumn; i++)
            {
                // Get the list of values in this column that aren't equal to zero.
                var nonZeroRows = matrix.ConstraintColumns[i].Where(value => value != 0);

                // If count equals one then this is a basic variable.
                if (nonZeroRows.Count() == 1)
                {
                    // Get the index of the row value.
                    var index = matrix.ConstraintColumns[i].IndexOf(nonZeroRows.Single());

                    // Check to see if the value of the basic variable is positive. If not,
                    // this is an infeasible solution.
                    if (matrix.Columns[i][index] * matrix.Columns[valuesColumn][index] < 0)
                    {
                        row = index;
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Performs row operations on the matrix until a basic feasible solution is found.
        /// </summary>
        /// <param name="row">A row with an infeasible basic variable.</param>
        /// <returns>Whether or not this matrix can be made feasible.</returns>
        private bool MakeFeasible(int row)
        {
            try
            {
                // Find the first (left-most) column in the given row with a positive value,
                // and use its column to pivot.
                var positive = matrix.CoeffRows[row].First(value => value > 0);
                var column = matrix.CoeffRows[row].IndexOf(positive);

                // Find the pivot row from the calculated column.
                row = CalculatePivotRow(column);

                return Pivot(row, column);
            }
            catch
            {
                // No optimal solution exists.
                return false;
            }
        }

        /// <summary>
        /// Optimizes the matrix by pivoting until all coefficients in the objective
        /// row are positive.
        /// </summary>
        /// <returns>Whether or not this matrix can be soloved.</returns>
        private bool SolveSimplex()
        {
            try
            {
                // Find the most negative value in the objective row, and use its column
                // as the pivot column.
                var min = matrix.CoeffRows[objRow].Where(value => value < 0).Min();
                var column = matrix.CoeffRows[objRow].IndexOf(min);

                // Find the pivot row from the calculated column.
                var row = CalculatePivotRow(column);

                return Pivot(row, column);
            }
            catch
            {
                // No optimal solution exists.
                return false;
            }
        }

        /// <summary>
        /// Returns the row with the minimum quotient when dividing the value 
        /// in the last column of the matrix by the value in the given column.
        /// </summary>
        /// <param name="column">The pivot column.</param>
        /// <returns>The pivot row.</returns>
        private int CalculatePivotRow(int column)
        {
            int row = -1;
            double min = double.PositiveInfinity;
            for (int i = 0; i < objRow; i++)
            {
                if (matrix.Rows[i][column] <= 0) continue;

                var quotient = matrix.Rows[i][valuesColumn] / matrix.Rows[i][column];
                if (quotient < min)
                {
                    min = quotient;
                    row = i;
                }
            }
            return row;
        }

        /// <summary>
        /// Performs row operations on the matrix to make the variable in the given
        /// column a basic variable.
        /// </summary>
        /// <param name="row">The pivot row.</param>
        /// <param name="column">The pivot column.</param>
        /// <returns>Whether or not the pivot was successful.</returns>
        private bool Pivot(int row, int column)
        {
            // Check to see if a valid row and column was given.
            if (row < 0 || column < 0) return false;

            // Divide the pivot row by the pivot value.
            var pivot = matrix.Rows[row][column];
            for (int i = 0; i < columns; i++)
            {
                // Will throw a DivideByZeroException if pivot equals zero.
                matrix.Rows[row][i] /= pivot;
            }

            // Subtract the given row times a multiplier from every other
            // row to make the variable in the given column a basic variable.
            for (int i = 0; i < rows; i++)
            {
                var multiplier = matrix.Rows[i][column];
                if (i == row || multiplier == 0) continue;
                
                for (int j = 0; j < columns; j++)
                {
                    matrix.Rows[i][j] -= multiplier * matrix.Rows[row][j];
                }
            }

            return true;
        }

        /// <summary>
        /// Computes the optimal solution for the given variables from the matrix. 
        /// </summary>
        /// <returns>The optimal solution.</returns>
        private IDictionary<object, double> GetSolution()
        {
            var solution = new Dictionary<object, double>();
            for (int i = 0; i < variablesDict.Count; i++)
            {
                // Get the variable identifier.
                var variable = variablesDict[i];

                // Get the list of values in this column that aren't equal to zero.
                var nonZeroRows = matrix.ConstraintColumns[i].Where(value => value != 0);

                // If count equals one then this is a basic variable.
                if (nonZeroRows.Count() == 1)
                {
                    // Get the index of the row value.
                    var index = matrix.ConstraintColumns[i].IndexOf(nonZeroRows.Single());

                    // Check to see if the value of the basic variable is positive. If not,
                    // this is an infeasible solution.
                    solution.Add(variable, matrix.Columns[i][index] * matrix.Columns[valuesColumn][index]);
                }
                else
                {
                    solution.Add(variable, 0);
                }
            }
            return solution;
        }

        /// <summary>
        /// Prints the matrix for internal debugging.
        /// </summary>
        private void PrintMatrix()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    System.Diagnostics.Debug.Write(matrix.Rows[i][j] + "\t");
                }
                System.Diagnostics.Debug.Write("\n");
            }
            System.Diagnostics.Debug.Write("\n");
        }

        #endregion

    }
}
