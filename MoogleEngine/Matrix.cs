namespace MoogleEngine;

public class Matrix
{
    private double[,] elements;

    public int Rows
    {
        get { return elements.GetLength(0); }
    }

    public int Columns
    {
        get { return elements.GetLength(1); }
    }

    public double this[int i, int j]
    {
        get { return elements[i, j]; }
        set { elements[i, j] = value; }
    }

    public Matrix(int rows, int columns)
    {
        elements = new double[rows, columns];
    }

    public static Matrix operator +(Matrix m1, Matrix m2)
    {
        if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
        {
            throw new ArgumentException("Matrices must have the same dimensions.");
        }
        Matrix result = new Matrix(m1.Rows, m1.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                result[i, j] = m1[i, j] + m2[i, j];
            }
        }
        return result;
    }

    public static Matrix operator -(Matrix m1, Matrix m2)
    {
        if (m1.Rows != m2.Rows || m1.Columns != m2.Columns)
        {
            throw new ArgumentException("Matrices must have the same dimensions.");
        }
        Matrix result = new Matrix(m1.Rows, m1.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                result[i, j] = m1[i, j] - m2[i, j];
            }
        }
        return result;
    }

    public static Matrix operator *(Matrix m1, Matrix m2)
    {
        if (m1.Columns != m2.Rows)
        {
            throw new ArgumentException("The number of columns in the first matrix must match the number of rows in the second matrix.");
        }
        Matrix result = new Matrix(m1.Rows, m2.Columns);
        for (int i = 0; i < result.Rows; i++)
        {
            for (int j = 0; j < result.Columns; j++)
            {
                for (int k = 0; k < m1.Columns; k++)
                {
                    result[i, j] += m1[i, k] * m2[k, j];
                }
            }
        }
        return result;
    }

    public static Vector operator *(Matrix m1, Vector v1)
    {
        if (m1.Columns != v1.Dimension)
        {
            throw new ArgumentException("The number of columns of the matrix must match the dimension of the vector.");
        }
        Vector result = new Vector(v1.Dimension);
        for (int i = 0; i < m1.Rows; i++)
        {
            for (int j = 0; j < result.Dimension; j++)
            {
                result[i] += m1[i,j] * v1[j];
            }
        }
        return result;
    }

    public static Matrix operator *(Matrix m, double scalar){
        Matrix result = new Matrix(m.Rows,m.Columns);
        for (int i = 0; i < m.Rows; i++)
        {
            for (int j = 0; j < m.Columns; j++)
            {
                result[i,j] = scalar * m[i,j];
            }
        }
        return result;
    }

    public Matrix Transpose()
    {
        Matrix result = new Matrix(Columns, Rows);
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                result[j, i] = elements[i, j];
            }
        }
        return result;
    }

    public Vector GetRow(int index)
    {
        Vector elements = new Vector(Columns);
        for (int i = 0; i < Columns; i++)
            elements[i] = this[index, i];
        return elements;
    }

        public Vector GetCol(int index)
        {
            Vector elements = new Vector(Rows);
            for (int i = 0; i < Rows; i++)
                elements[i] = this[i, index];
            return elements;
        }

}