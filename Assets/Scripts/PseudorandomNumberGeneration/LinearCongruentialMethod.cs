using System;
using System.Collections.Generic;

public class LinearCongruentialMethod
{
    // Lista para almacenar los valores de Ri
    private List<double> riValues;

    private int xo; // Semilla inicial
    private int k;  // Multiplicador base
    private int c;  // Incremento
    private long g;  // Parámetro del módulo
    private int iterations; // Número de iteraciones

    // Constructor
    public LinearCongruentialMethod(int xo, int k, int c, long g, int iterations)
    {
        this.riValues = new List<double>();
        this.xo = xo;
        this.k = k;
        this.c = c;
        this.g = g;
        this.iterations = iterations;
        FillRiValues();
    }

    // Método para generar el valor 'a'
    private int GenerateAValue()
    {
        return 1 + (2 * k);
    }

    // Método para determinar la cantidad de números posibles (módulo)
    private int DetermineNumberAmount()
    {
        return (int)Math.Pow(2, g);
    }

    // Método para calcular y almacenar los valores Ri
    public void FillRiValues()
    {
        int a = GenerateAValue();
        int amount = DetermineNumberAmount();
        double xi = xo; // Comenzamos con la semilla

        for (int i = 0; i < iterations; i++)
        {
            xi = ((a * xi) + c) % amount; // Calculamos el valor Xi
            double riValue = xi / (amount - 1); // Calculamos el valor Ri

            // Solo almacenamos Ri si no es 0 ni 1
            if (riValue != 0 && riValue != 1)
            {
                riValues.Add(Math.Round(riValue, 5)); // Almacenamos Ri
            }
        }
    }

    // Método para obtener el conjunto de valores Ri
    public List<double> GetRiValues()
    {
        return riValues;
    }
}
