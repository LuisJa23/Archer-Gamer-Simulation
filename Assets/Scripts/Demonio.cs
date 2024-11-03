using UnityEngine;

public class DemonBehavior : MonoBehaviour
{
    public Transform player; 
    public float detectionDistance = 10f; // Distancia máxima para detectar al jugador
    public float chaseSpeed = 0.5f; // Velocidad del demonio al perseguir
    public int health = 1; // Vida del demonio

    private bool isStopped;

    private Animator Animator;
    private Rigidbody2D Rigidbody2D;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.gravityScale = 0;
        Animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    void Update()
    {
        Vector3 directionToDemon = transform.position - player.position;
        float distanceToPlayer = directionToDemon.magnitude;

        // Verifica si el jugador está dentro del rango de detección
        if (distanceToPlayer <= detectionDistance)
        {
            // Determina si el jugador está mirando en la dirección del demonio
            bool playerLookingAtDemon = (player.localScale.x > 0 && transform.position.x > player.position.x) ||
                                         (player.localScale.x < 0 && transform.position.x < player.position.x);

            if (!playerLookingAtDemon)
            {
                Animator.SetBool("isStopped", false);
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // Cambia la dirección del demonio según el movimiento
                if (directionToPlayer.x > 0)
                {
                    transform.localScale = new Vector3(2f, 2f, 2); // Mirando a la derecha
                }
                else if (directionToPlayer.x < 0)
                {
                    transform.localScale = new Vector3(-2f, 2f, 2); // Mirando a la izquierda
                }

                transform.Translate(directionToPlayer * chaseSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                Animator.SetBool("isStopped", true);
            }
            // Si el jugador está mirando al demonio, el demonio se queda quieto
        }
    }

    public void TakeDamage()
    {
        health--; // Resta 1 vida al demonio
        Debug.Log("Demon took damage. Remaining health: " + health);

        if (health <= 0)
        {
            Disappear(); // Si no tiene vida, desaparece
        }
    }

    private void Disappear()
    {
        // Aquí puedes agregar un efecto de desaparición si lo deseas
        gameObject.SetActive(false); // Desactiva el objeto demonio
        Debug.Log("Demon has disappeared.");
    }
}