namespace MoogleEngine;
public class Matrix
{
    private int[,] matriz;
    private int filas;
    private int columnas;

    public Matrix(int[,] matriz)
    {
        this.matriz = matriz;
        this.filas = matriz.GetLength(0);
        this.columnas = matriz.GetLength(1);
    }

    public static Matrix operator +(Matrix matriz1, Matrix matriz2)
    {
        if (matriz1.filas != matriz2.filas || matriz1.columnas != matriz2.columnas)
        {
            throw new ArgumentException("Las matrices deben tener las mismas dimensiones");
        }

        int[,] resultado = new int[matriz1.filas, matriz1.columnas];

        for (int i = 0; i < matriz1.filas; i++)
        {
            for (int j = 0; j < matriz1.columnas; j++)
            {
                resultado[i, j] = matriz1.matriz[i, j] + matriz2.matriz[i, j];
            }
        }

        return new Matrix(resultado);
    }

    public int[] MultiplicarPorVector(int[] vector)
    {
        if (this.columnas != vector.Length)
        {
            throw new ArgumentException("El vector debe tener la misma cantidad de elementos que columnas en la matriz");
        }

        int[] resultado = new int[this.filas];

        for (int i = 0; i < this.filas; i++)
        {
            int suma = 0;

            for (int j = 0; j < this.columnas; j++)
            {
                suma += this.matriz[i, j] * vector[j];
            }

            resultado[i] = suma;
        }

        return resultado;
    }

    public Matrix MultiplicarPorEscalar(int escalar)
    {
        int[,] resultado = new int[this.filas, this.columnas];

        for (int i = 0; i < this.filas; i++)
        {
            for (int j = 0; j < this.columnas; j++)
            {
                resultado[i, j] = this.matriz[i, j] * escalar;
            }
        }

        return new Matrix(resultado);
    }

    public int this[int i, int j]
    {
        get { return this.matriz[i, j]; }
        set { this.matriz[i, j] = value; }
    }
}
