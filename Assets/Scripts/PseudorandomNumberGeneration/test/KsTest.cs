using System;
using System.Collections.Generic;
using System.Linq;

public class KsTest
{
    public bool CheckTest(List<double> datos)
    {
        // Ordenar los datos
        datos.Sort();

        // Calcular las desviaciones D+ y D-
        int n = datos.Count;
        List<double> D_positivos = new List<double>();
        List<double> D_negativos = new List<double>();

        for (int i = 0; i < n; i++)
        {
            // Cálculo de D+ y D-
            D_positivos.Add(((i + 1.0) / n) - datos[i]);
            D_negativos.Add(datos[i] - (i / (double)n));
        }

        // Encontrar el máximo de D+ y D-
        double D_positivo_max = D_positivos.Max();
        double D_negativo_max = D_negativos.Max();
        double D = Math.Max(D_positivo_max, D_negativo_max);

        // Calcular el valor crítico D_alfa (para un nivel de significancia del 5%)
        double D_alfa = 1.36 / Math.Sqrt(n);

        // Comparar D con D_alfa
        return D <= D_alfa;
    }
}
