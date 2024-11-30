using System.Collections.Generic;

public class RandomNumberValidator
{
    // Propiedad para almacenar los números generados
    private List<double> numbers;

    private int counter = 0;

    // Constructor que toma una lista de números
    public RandomNumberValidator()
    {
        LinearCongruentialMethod linearCongruentialMethod = new LinearCongruentialMethod(1, 1664525, 1013904223, 32, 1000);

        this.numbers = linearCongruentialMethod.GetRiValues();
    }

    // Función que valida si los números pasan todas las pruebas
    public List<double> GetValidNumbers()
    {
        // Creamos instancias de las pruebas
        AverageTest at = new AverageTest(numbers.ToArray());
        ChiSquaredTest ct = new ChiSquaredTest(numbers);
        KsTest kt = new KsTest();
        VarianceTest vt = new VarianceTest(numbers.ToArray());
        PokerTest pt = new PokerTest(numbers);

        // Lista para almacenar los números válidos que pasan todas las pruebas
        List<double> validNumbers = new List<double>();

        // Verificamos si todos los test son exitosos
        if (at.CheckTest() && ct.CheckTest() && kt.CheckTest(numbers) && vt.CheckTest() && pt.CheckTest())
        {
            validNumbers = numbers;
        }
        return numbers;
    }

    public double GetNextNumber()
    {
        if (numbers.Count == 0)
        {
            return -1; // No hay números válidos en la lista.
        }

        double number = numbers[counter];
        counter = (counter + 1) % numbers.Count; // Reinicia al inicio si se excede.
        return number;
    }

    public double GetNextRandom()
    {
        if (numbers.Count == 0)
        {
            return -1; // No hay números válidos en la lista.
        }

        System.Random random = new System.Random();
        int randomIndex = random.Next(0, numbers.Count);
        return numbers[randomIndex];
    }
}