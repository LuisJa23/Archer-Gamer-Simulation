using System;
using System.Linq;
using System.Collections.Generic;

public class PokerTest
{
    // Probabilidades de cada mano de poker según la tabla de poker
    private readonly double[] prob = { 0.3024, 0.504, 0.108, 0.072, 0.009, 0.0045, 0.0001 };
    private readonly int[] oi = { 0, 0, 0, 0, 0, 0, 0 }; // Frecuencias observadas
    private List<double> ei = new List<double>(); // Frecuencias esperadas
    private List<double> eid = new List<double>(); // Resultado (oi - ei)^2 / ei
    private double totalSum = 0.0; // Suma total de (oi - ei)^2 / ei
    private double chiReverse; // Valor crítico de chi-cuadrado
    private int n; // Número de elementos en la secuencia de números pseudoaleatorios
    private List<double> riNums;

    public PokerTest(List<double> riNums)
    {
        n = riNums.Count;
        chiReverse = ChiSquaredCriticalValue(0.05, 6); // Chi-cuadrado para 6 grados de libertad y un nivel de significancia de 0.05
        this.riNums = riNums;
    }

    public bool CheckTest()
    {
        CalculateOi(riNums);
        CalculateEi();
        CalculateEid();
        CalculateTotalSum();

        return totalSum > chiReverse;
    }

    private void CalculateOi(List<double> riNums)
    {
        foreach (var n in riNums)
        {
            // Verificar que 'n' esté dentro del rango [0, 1]
            if (n < 0 || n > 1)
            {
                continue; // Descartamos los valores fuera del rango
            }

            // Convertir el número a una cadena de texto y trabajar con la parte decimal
            var numStr = n.ToString();
            int index = GetPokerHandIndex(numStr); // Obtener el índice de la mano de poker
            if (index >= 0 && index < 6) // Comprobamos que el índice esté dentro del rango de manos de poker
            {
                oi[index]++;
            }
        }
    }

    private int GetPokerHandIndex(string numStr)
    {
        if (AllDiff(numStr)) return 0; // Todas diferentes
        if (AllSame(numStr)) return 6; // Todas iguales
        if (FourOfAKind(numStr)) return 5; // Cuatro del mismo valor (Poker)
        if (OneThreeOfAKindAndOnePair(numStr)) return 4; // Una tercia y un par (Full House)
        if (OnlyThreeOfAKind(numStr)) return 3; // Solo una tercia
        if (TwoPairs(numStr)) return 2; // Dos pares
        if (OnlyOnePair(numStr)) return 1; // Solo un par
        return -1; // Si no se puede clasificar, retornar -1
    }

    private bool AllDiff(string numStr) => numStr.Distinct().Count() == numStr.Length;

    private bool AllSame(string numStr) => numStr.Distinct().Count() == 1;

    private bool FourOfAKind(string numStr) => numStr.GroupBy(c => c).Any(g => g.Count() == 4);

    private bool TwoPairs(string numStr) => numStr.GroupBy(c => c).Count(g => g.Count() == 2) == 2;

    private bool OneThreeOfAKindAndOnePair(string numStr)
    {
        var counts = numStr.GroupBy(c => c).Select(g => g.Count()).ToList();
        return counts.Contains(3) && counts.Contains(2);
    }

    private bool OnlyOnePair(string numStr) => numStr.GroupBy(c => c).Count(g => g.Count() == 2) == 1;

    private bool OnlyThreeOfAKind(string numStr) => numStr.GroupBy(c => c).Count(g => g.Count() == 3) == 1;

    private void CalculateEi()
    {
        for (int i = 0; i < 6; i++)
        {
            ei.Add(prob[i] * n);
        }
    }

    private void CalculateEid()
    {
        for (int i = 0; i < 6; i++)
        {
            if (ei[i] != 0)
            {
                eid.Add(Math.Pow(oi[i] - ei[i], 2) / ei[i]);
            }
        }
    }

    private void CalculateTotalSum()
    {
        totalSum = eid.Sum();
    }

    private double ChiSquaredCriticalValue(double alpha, int degreesOfFreedom)
    {
        // Aquí se puede utilizar una tabla de chi-cuadrado o una función matemática específica.
        // Este es un valor aproximado para un nivel de significancia de 0.05 y 6 grados de libertad.
        return 12.5916; // Chi-cuadrado de 6 grados de libertad para 0.05 de significancia
    }
}
