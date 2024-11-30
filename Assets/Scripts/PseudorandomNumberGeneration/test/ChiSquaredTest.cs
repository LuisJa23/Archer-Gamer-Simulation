using System;
using System.Collections.Generic;
using System.Linq;

public class ChiSquaredTest
{
    private List<double> riValues;
    private List<double> niValues;
    private List<double> intervalsValues;
    private List<int> frequencyObtained;
    private List<double> expectedFrequency;
    private List<double> chiSquaredValues;

    private int intervalsAmount;
    private double a;
    private double b;

    private double chiSquaredCriticalValue;
    private double chiSquaredSum;

    public ChiSquaredTest(List<double> riValues, int intervalsAmount = 8, double a = 8, double b = 10)
    {
        this.riValues = riValues;
        this.intervalsAmount = intervalsAmount;
        this.a = a;
        this.b = b;
        this.niValues = new List<double>();
        this.intervalsValues = new List<double>();
        this.frequencyObtained = new List<int>();
        this.expectedFrequency = new List<double>();
        this.chiSquaredValues = new List<double>();
    }

    private void FillNiValues()
    {
        foreach (var ri in riValues)
        {
            double value = a + (b - a) * ri;
            niValues.Add(value);
        }
    }

    private void SortNiArray()
    {
        niValues.Sort();
    }

    private double GetMinNiValue()
    {
        return niValues.Min();
    }

    private double GetMaxNiValue()
    {
        return niValues.Max();
    }

    private void FillIntervalsValues()
    {
        double minValue = GetMinNiValue();
        double maxValue = GetMaxNiValue();
        intervalsValues.Add(minValue);

        for (int i = 0; i < intervalsAmount; i++)
        {
            double value = Math.Round(intervalsValues[i] + (maxValue - minValue) / intervalsAmount, 5);
            intervalsValues.Add(value);
        }
    }

    private void FillFrequencies()
    {
        double expectedFreq = Math.Round((double)niValues.Count / intervalsAmount, 2);
        foreach (var interval in intervalsValues.SkipLast(1))
        {
            int counter = niValues.Count(ni => ni >= interval && ni < interval + (GetMaxNiValue() - GetMinNiValue()) / intervalsAmount);
            frequencyObtained.Add(counter);
            expectedFrequency.Add(expectedFreq);
        }
    }

    private void FillChiSquaredValues()
    {
        for (int i = 0; i < frequencyObtained.Count; i++)
        {
            double value = Math.Round(Math.Pow(frequencyObtained[i] - expectedFrequency[i], 2) / expectedFrequency[i], 2);
            chiSquaredValues.Add(value);
        }
    }

    private double ChiSquaredCriticalValue(int degreesOfFreedom, double marginOfError)
    {
        // Valores críticos aproximados de Chi-Cuadrado para margen de error 0.05
        var criticalValues = new Dictionary<int, double>
        {
            { 1, 3.841 },
            { 2, 5.991 },
            { 3, 7.815 },
            { 4, 9.488 },
            { 5, 11.070 },
            { 6, 12.592 },
            { 7, 14.067 },
            { 8, 15.507 },
            { 9, 16.919 },
            { 10, 18.307 }
        };

        return criticalValues.ContainsKey(degreesOfFreedom) 
            ? criticalValues[degreesOfFreedom] 
            : throw new ArgumentException("Grados de libertad fuera de rango.");
    }

    private double CalculateChiSquaredSum()
    {
        return chiSquaredValues.Sum();
    }

    public bool CheckTest()
    {
        FillNiValues();
        SortNiArray();
        FillIntervalsValues();
        FillFrequencies();
        FillChiSquaredValues();

        int degreesOfFreedom = intervalsAmount - 1;
        double marginOfError = 0.05;

        chiSquaredCriticalValue = ChiSquaredCriticalValue(degreesOfFreedom, marginOfError);
        chiSquaredSum = CalculateChiSquaredSum();

        return chiSquaredSum <= chiSquaredCriticalValue;
    }
}
