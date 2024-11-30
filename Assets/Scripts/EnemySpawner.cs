using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controlador de generación de enemigos en un área delimitada.
/// Permite gestionar la aparición aleatoria de enemigos dentro de un área definida
/// y limita la cantidad de enemigos en pantalla.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    // Límites del área de aparición
    private float minX, maxX, minY, maxY;

    [SerializeField] private Transform[] Spawners; // Puntos delimitadores del área de aparición
    [SerializeField] private float arrivalRate = 0.05f; // Tasa de llegadas (lambda) para determinar intervalos de spawn
    [SerializeField] private List<GameObject> enemyPrefabs; // Lista de prefabs de enemigos disponibles

    private Queue<GameObject> enemyQueue = new Queue<GameObject>(); // Cola de enemigos para administrar el spawn
    private float nextSpawnTime; // Tiempo programado para la próxima aparición

    [SerializeField] private int maxEnemiesOnScreen = 5; // Número máximo de enemigos permitidos en pantalla

    private RandomNumberValidator randomNumberValidator = new RandomNumberValidator(); // Generador de números aleatorios validados
    [SerializeField] private GameObject specialDemonPrefab;

    /// <summary>
    /// Inicializa los valores del área de spawn y llena la cola de enemigos.
    /// </summary>
    void Start()
    {
        // Configurar límites del área basándose en las posiciones de los puntos delimitadores
        maxX = Spawners.Max(spawner => spawner.position.x);
        minX = Spawners.Min(spawner => spawner.position.x);
        maxY = Spawners.Max(spawner => spawner.position.y);
        minY = Spawners.Min(spawner => spawner.position.y);

        // Llenar la cola inicial de enemigos
        PopulateEnemyQueue();

        // Calcular el tiempo para el primer spawn usando distribución exponencial
        nextSpawnTime = Time.time + GetExponentialRandom(arrivalRate);
    }

    /// <summary>
    /// Método llamado cada frame. Controla el tiempo de aparición de enemigos.
    /// </summary>
    private void Update()
    {
        // Verifica si es momento de generar un nuevo enemigo y si no se ha alcanzado el límite en pantalla
        if (Time.time >= nextSpawnTime && GetActiveEnemyCount() < maxEnemiesOnScreen)
        {
            SpawnEnemy(); // Genera un nuevo enemigo
            nextSpawnTime = Time.time + GetExponentialRandom(arrivalRate); // Programa la próxima aparición
        }
    }

    /// <summary>
    /// Genera un número aleatorio usando la distribución exponencial.
    /// </summary>
    /// <param name="lambda">Tasa de llegada (lambda) que controla la frecuencia de aparición.</param>
    /// <returns>Un número aleatorio basado en la distribución exponencial.</returns>
    private float GetExponentialRandom(float lambda)
    {
        return -Mathf.Log((float)(1 - randomNumberValidator.GetNextNumber())) / lambda;
    }

    /// <summary>
    /// Obtiene la cantidad actual de enemigos activos en la escena.
    /// </summary>
    /// <returns>El número de enemigos activos.</returns>
    private int GetActiveEnemyCount()
    {
        return GameObject.FindObjectsByType<DemonBehavior>(FindObjectsSortMode.None).Length;
    }

    /// <summary>
    /// Genera un enemigo en una posición aleatoria dentro del área delimitada.
    /// </summary>
    private void SpawnEnemy()
    {
        // Generar una posición aleatoria dentro del área de spawn
        Vector2 position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        // Verifica si la cola tiene enemigos disponibles
        if (enemyQueue.Count > 0)
        {
            // Extrae el primer enemigo de la cola e instáncialo en la posición generada
            GameObject enemyPrefab = enemyQueue.Dequeue();
            Instantiate(enemyPrefab, position, Quaternion.identity);

            // Si la cola está vacía después de extraer un enemigo, la rellena nuevamente
            if (enemyQueue.Count == 0)
            {
                PopulateEnemyQueue();
            }
        }
        else
        {
            Debug.LogWarning("Enemy queue is empty. No enemies spawned."); // Advertencia si la cola está vacía
        }
    }

    /// <summary>
    /// Llena la cola de enemigos utilizando los prefabs disponibles.
    /// </summary>
    private void PopulateEnemyQueue()
    {
        foreach (var prefab in enemyPrefabs)
        {
            enemyQueue.Enqueue(prefab); // Agrega cada prefab a la cola
        }

        // Opcional: Mezcla la cola para generar aleatoriedad en el orden de aparición
        ShuffleQueue();
    }

    /// <summary>
    /// Mezcla el contenido de la cola de enemigos para hacer más impredecible su orden de aparición.
    /// </summary>
    private void ShuffleQueue()
    {
        List<GameObject> tempList = enemyQueue.ToList(); // Convierte la cola en una lista temporal
        enemyQueue.Clear(); // Vacía la cola original

        // Mezcla aleatoriamente los elementos y los agrega nuevamente a la cola
        while (tempList.Count > 0)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            enemyQueue.Enqueue(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }
}
