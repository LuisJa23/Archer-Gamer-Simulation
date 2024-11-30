using System;
using System.Linq;

public class VarianceTest
{
    private double[] riNumbers;
    private double variance;
    private double alpha = 0.05;
    private double average;
    private int n;
    private double superiorLimit;
    private double inferiorLimit;
    private double chiSquare1;
    private double chiSquare2;

    public VarianceTest(double[] riNumbers)
    {
        this.riNumbers = riNumbers;
        this.n = riNumbers.Length;
    }

    // Calcular la varianza
    private void CalculateVariance()
    {
        variance = riNumbers.Select(x => Math.Pow(x - average, 2)).Average();
    }

    // Calcular el promedio
    private void CalculateAverage()
    {
        average = riNumbers.Average();
    }

    // Calcular chi-cuadrado para el límite inferior
    private void CalculateChiSquare1()
    {
        chiSquare1 = ChiSquareInverse(alpha / 2, n - 1);
    }

    // Calcular chi-cuadrado para el límite superior
    private void CalculateChiSquare2()
    {
        chiSquare2 = ChiSquareInverse(1 - alpha / 2, n - 1);
    }

    // Función para obtener el valor crítico de chi-cuadrado (simulando la distribución chi-cuadrado)
    private double ChiSquareInverse(double p, int df)
    {
        // Este es un ejemplo simple. En la práctica, puedes usar una librería como MathNet.Numerics
        // para calcular el valor inverso de chi-cuadrado o usar valores predefinidos.
        return Math.Sqrt(df * (1 - p));  // Esto es una aproximación simple
    }

    // Calcular el límite inferior de la varianza
    private void CalculateInferiorLimit()
    {
        inferiorLimit = chiSquare1 / (12 * (n - 1));
    }

    // Calcular el límite superior de la varianza
    private void CalculateSuperiorLimit()
    {
        superiorLimit = chiSquare2 / (12 * (n - 1));
    }

    // Realizar la prueba de varianza
    public bool CheckTest()
    {
        CalculateAverage();
        CalculateVariance();
        CalculateChiSquare1();
        CalculateChiSquare2();
        CalculateSuperiorLimit();
        CalculateInferiorLimit();

        if (inferiorLimit <= variance && variance <= superiorLimit)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }


}


