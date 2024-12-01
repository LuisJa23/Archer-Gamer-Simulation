using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    private float minX, maxX, minY, maxY;

    [SerializeField] private Transform[] Spawners;
    [SerializeField] private float arrivalRate = 0.05f;
    [SerializeField] private List<GameObject> enemyPrefabs;

    private Queue<GameObject> enemyQueue = new Queue<GameObject>();
    private float nextSpawnTime;

    [SerializeField] private int initialMaxEnemiesOnScreen = 5; // Máximo inicial
    private int maxEnemiesOnScreen;
    private const int maxLimitEnemies = 10; // Máximo absoluto
    private int previousMilestone = 0;

    [SerializeField] private GameObject specialDemonPrefab;

    private RandomNumberValidator randomNumberValidator = new RandomNumberValidator();

    void Start()
    {
        maxX = Spawners.Max(spawner => spawner.position.x);
        minX = Spawners.Min(spawner => spawner.position.x);
        maxY = Spawners.Max(spawner => spawner.position.y);
        minY = Spawners.Min(spawner => spawner.position.y);

        maxEnemiesOnScreen = initialMaxEnemiesOnScreen;

        PopulateEnemyQueue();
        nextSpawnTime = Time.time + GetExponentialRandom(arrivalRate);
    }

    private void Update()
    {
        int currentScore = ScoreText.scoreValue;

        // Comprobar si hemos alcanzado un múltiplo de 100 para generar un demonio especial
        if (currentScore / 100 > previousMilestone)
        {
            SpawnSpecialDemon();
            previousMilestone = currentScore / 100;

            // Incrementar el número máximo de enemigos en pantalla hasta un límite
            maxEnemiesOnScreen = Mathf.Min(maxLimitEnemies, maxEnemiesOnScreen + 1);
        }

        // Generar enemigos regulares si no se excede el límite en pantalla
        if (Time.time >= nextSpawnTime && GetActiveEnemyCount() < maxEnemiesOnScreen)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + GetExponentialRandom(arrivalRate);
        }
    }

    private float GetExponentialRandom(float lambda)
    {
        return -Mathf.Log((float)(1 - randomNumberValidator.GetNextNumber())) / lambda;
    }

    private int GetActiveEnemyCount()
    {
        return GameObject.FindObjectsByType<DemonBehavior>(FindObjectsSortMode.None).Length;
    }

    private void SpawnEnemy()
    {
        Vector2 position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

        if (enemyQueue.Count > 0)
        {
            GameObject enemyPrefab = enemyQueue.Dequeue();
            Instantiate(enemyPrefab, position, Quaternion.identity);

            if (enemyQueue.Count == 0)
            {
                PopulateEnemyQueue();
            }
        }
        else
        {
            Debug.LogWarning("Enemy queue is empty. No enemies spawned.");
        }
    }

    private void PopulateEnemyQueue()
    {
        foreach (var prefab in enemyPrefabs)
        {
            enemyQueue.Enqueue(prefab);
        }
        ShuffleQueue();
    }

    private void ShuffleQueue()
    {
        List<GameObject> tempList = enemyQueue.ToList();
        enemyQueue.Clear();

        while (tempList.Count > 0)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            enemyQueue.Enqueue(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }

    private void SpawnSpecialDemon()
    {
        Vector2 position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        GameObject specialDemon = Instantiate(specialDemonPrefab, position, Quaternion.identity);

        DemonBehavior demonBehavior = specialDemon.GetComponent<DemonBehavior>();
        if (demonBehavior != null)
        {
            demonBehavior.isSpecialDemon = true;
        }

        Debug.Log("Special demon spawned!");
    }
}
