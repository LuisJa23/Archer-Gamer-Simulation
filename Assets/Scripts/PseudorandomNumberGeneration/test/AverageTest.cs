using System;
using System.Linq; // Para usar funciones como Average

public class AverageTest
{
    private double[] riNums; // Arreglo de números pseudoaleatorios
    private double alpha; // Nivel de significancia (error permitido)
    private int n; // Tamaño del conjunto de números
    private double average; // Promedio calculado
    private double z; // Valor crítico Z
    private double superiorLimit; // Límite superior
    private double inferiorLimit; // Límite inferior

    // Constructor que recibe los números Ri
    public AverageTest(double[] riNums, double alpha = 0.05)
    {
        this.riNums = riNums;
        this.alpha = alpha;
        this.n = riNums.Length;
        this.average = 0.0;
        this.z = 0.0;
        this.superiorLimit = 0.0;
        this.inferiorLimit = 0.0;
    }

    // Calcula el promedio del conjunto de números
    private void CalculateAverage()
    {
        if (n > 0)
        {
            average = riNums.Average();
        }
    }

    // Calcula el valor Z para el nivel de significancia dado
    private void CalculateZ()
    {
        // Usamos valores Z predefinidos basados en tablas estándar
        if (Math.Abs(alpha - 0.05) < 1e-6) // Nivel de confianza del 95%
        {
            z = 1.96;
        }
        else if (Math.Abs(alpha - 0.01) < 1e-6) // Nivel de confianza del 99%
        {
            z = 2.576;
        }
        else
        {
            throw new InvalidOperationException("No se tiene un valor crítico Z predefinido para este nivel de significancia.");
        }
    }

    // Calcula el límite superior
    private void CalculateSuperiorLimit()
    {
        if (n > 0)
        {
            superiorLimit = 0.5 + (z * (1 / Math.Sqrt(12 * n)));
        }
    }

    // Calcula el límite inferior
    private void CalculateInferiorLimit()
    {
        if (n > 0)
        {
            inferiorLimit = 0.5 - (z * (1 / Math.Sqrt(12 * n)));
        }
    }

    // Realiza la prueba y devuelve si pasa o no (true si pasa, false si no pasa)
    public bool CheckTest()
    {
        CalculateAverage();
        CalculateZ();
        CalculateSuperiorLimit();
        CalculateInferiorLimit();

        return inferiorLimit <= average && average <= superiorLimit;
    }
}
