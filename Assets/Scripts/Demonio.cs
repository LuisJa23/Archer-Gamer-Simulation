using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DemonBehavior : MonoBehaviour
{
    public Transform player; 
    public float detectionDistance = 10f; // Distancia máxima para detectar al jugador
    public float chaseSpeed = 0.7f; // Velocidad del demonio al perseguir
    public int health = 1; // Vida del demonio
    public GameObject heartPrefab; // Prefab del corazón (OneHeart)
    public GameObject speedBoostPrefab; // Prefab para el aumento de velocidad

    private Animator animator;
    private Rigidbody2D rb2D;

    private RandomNumberValidator randomNumberValidator = new RandomNumberValidator();

    private int score = 10; 
    public bool isSpecialDemon = false; // Identifica si este demonio es especial


    private int currentState = 0; // Estado inicial: 0 (no generar ítem)
   

    void Start()
{
    rb2D = GetComponent<Rigidbody2D>();
    rb2D.gravityScale = 0;
    animator = GetComponent<Animator>();

    if (player == null)
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Si es un demonio especial, ajusta su tamaño y color
    if (isSpecialDemon)
    {
        GetComponent<SpriteRenderer>().color = Color.yellow; // Cambiar a dorado
        Debug.Log("Special demon initialized with larger size and golden color.");
    }
}


    void Update()
{
    Vector3 directionToPlayer = player.position - transform.position;
    float distanceToPlayer = directionToPlayer.magnitude;

    if (distanceToPlayer <= detectionDistance)
    {
        animator.SetBool("isStopped", false);
        Vector3 directionToMove = directionToPlayer.normalized;

        if (directionToMove.x > 0) // Mirando a la derecha
        {
            transform.localScale = isSpecialDemon 
                ? new Vector3(4f, 4f, 1f) // Demonio especial: tamaño x5 base (2f * 5 = 10f)
                : new Vector3(2f, 2f, 2f); // Demonio normal: tamaño base
        }
        else if (directionToMove.x < 0) // Mirando a la izquierda
        {
            transform.localScale = isSpecialDemon 
                ? new Vector3(-4f, 4f, 1f) // Demonio especial: tamaño x5 invertido
                : new Vector3(-2f, 2f, 2f); // Demonio normal: tamaño base invertido
        }

        // Movimiento del demonio hacia el jugador
        transform.Translate(directionToMove * chaseSpeed * Time.deltaTime, Space.World);
    }
    else
    {
        animator.SetBool("isStopped", true); // El demonio se detiene si está fuera del rango
    }
}


    public void TakeDamage()
    {
        health--;
        Debug.Log("Demon took damage. Remaining health: " + health);

        if (health <= 0)
        {
            Disappear();
        }
    }

    private void Disappear()
    {
        ScoreText.scoreValue += score;
        DropItem(); 
        gameObject.SetActive(false); 
        player.GetComponent<Arquero>().OnDemonKilled();
        Debug.Log("Demon has disappeared.");
    }

    private void DropItem()
    {
        // Determinar el próximo estado utilizando la cadena de Markov
        currentState = GetNextState(currentState);

        switch (currentState)
        {
            case 1:
                SpawnHeart();
                break;
            case 2:
                SpawnSpeedBoost();
                break;
            default:
                Debug.Log("No se generó ningún ítem.");
                break;
        }
    }

    private int GetNextState(int currentState)
{
    // Generar un nuevo número aleatorio cada vez que se llame al método
    float random = (float)randomNumberValidator.GetNextRandom(); 
    

    // Lógica de transición entre estados basada en el valor del número aleatorio
    if (currentState == 0) // Estado: Sin generar ítem
    {
        if (random < 0.3f) return 2; // 30% a corazón
        else if (random < 0.6f) return 1; // 30% a velocidad
        else return 0; // 40% se queda igual
    }
    else if (currentState == 1) // Estado: Corazón
    {
        if (random < 0.2f) return 1; // 20% a corazón
        else if (random < 0.4f) return 2; // 20% a velocidad
        else return 0; // 60% a sin generar
    }
    else if (currentState == 2) // Estado: Velocidad
    {
        if (random < 0.2f) return 1; // 20% a corazón
        else if (random < 0.4f) return 2; // 20% a velocidad
        else return 0; // 60% a sin generar
    }

    return 0; // Por defecto, no generar nada
}


    private void SpawnHeart()
    {
        if (heartPrefab != null)
        {
            Instantiate(heartPrefab, transform.position, Quaternion.identity);
            Debug.Log("¡El demonio ha dejado caer un Corazón!");
        }
        else
        {
            Debug.LogWarning("Prefab de Corazón no asignado!");
        }

        
    }

    private void SpawnSpeedBoost()
    {
        if (speedBoostPrefab != null)
        {
            Instantiate(speedBoostPrefab, transform.position, Quaternion.identity);
            Debug.Log("¡El demonio ha dejado caer un Aumento de Velocidad!");
        }
        else
        {
            Debug.LogWarning("Prefab de Aumento de Velocidad no asignado!");
        }
    }
}
