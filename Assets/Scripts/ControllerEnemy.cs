using UnityEngine;
using System.Linq;

public class ControllerEnemy : MonoBehaviour
{
    private float minX, maxX, minY, maxY;   

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private GameObject[] enemies;


    //Tiempo de aparicion, agregar un sistemas de llegadas

    [SerializeField] private float timeEnemy;
    private float timeBetweenEnemies;



    void Start()
    {
        maxX = waypoints.Max(point => point.position.x);
        minX = waypoints.Min(point => point.position.x);
        maxY = waypoints.Max(point => point.position.y);
        minY = waypoints.Min(point => point.position.y);

    }

    // Update is called once per frame
    void Update()
    {
        timeBetweenEnemies += Time.deltaTime;


        if (timeBetweenEnemies >= timeEnemy)
        {
            timeBetweenEnemies = 0;
            SpawnEnemy();
        }
    }


    private void SpawnEnemy()
    {
       
        int randomEnemy = Random.Range(0, enemies.Length);

        Vector2 ramdomPosition = new Vector2(Random.Range(minY, maxY), Random.Range(minX, maxX));
        Instantiate(enemies[randomEnemy], ramdomPosition, Quaternion.identity);
    }
}
