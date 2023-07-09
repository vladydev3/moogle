namespace MoogleEngine;

public class Vector
{
    private double[] components;

    public int Dimension
    {
        get { return components.Length; }
    }

    public double this[int i]
    {
        get { return components[i]; }
        set { components[i] = value; }
    }

    public Vector(params double[] components)
    {
        this.components = components;
    }


    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.Dimension != v2.Dimension)
        {
            throw new ArgumentException("Vectors must have the same dimension.");
        }
        double[] result = new double[v1.Dimension];
        for (int i = 0; i < v1.Dimension; i++)
        {
            result[i] = v1[i] + v2[i];
        }
        return new Vector(result);
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
        if (v1.Dimension != v2.Dimension)
        {
            throw new ArgumentException("Vectors must have the same dimension.");
        }
        double[] result = new double[v1.Dimension];
        for (int i = 0; i < v1.Dimension; i++)
        {
            result[i] = v1[i] - v2[i];
        }
        return new Vector(result);
    }

    public static double operator *(Vector v1, Vector v2)
    {
        if (v1.Dimension != v2.Dimension)
        {
            throw new ArgumentException("Vectors must have the same dimension.");
        }
        double result = 0;
        for (int i = 0; i < v1.Dimension; i++)
        {
            result += v1[i] * v2[i];
        }
        return result;
    }

    public static Vector operator *(double scalar, Vector v)
    {
        double[] result = new double[v.Dimension];
        for (int i = 0; i < v.Dimension; i++)
        {
            result[i] = scalar * v[i];
        }
        return new Vector(result);
    }

    public static Vector operator *(Vector v, double scalar)
    {
        return scalar * v;
    }

    public double Magnitude()
    {
        double result = 0;
        for (int i = 0; i < Dimension; i++)
        {
            result += components[i] * components[i];
        }
        return Math.Sqrt(result);
    }

    public Vector Normalize()
    {
        double mag = Magnitude();
        if (mag == 0)
        {
            throw new DivideByZeroException("Cannot normalize a zero vector.");
        }
        return this * (1 / mag);
    }

}