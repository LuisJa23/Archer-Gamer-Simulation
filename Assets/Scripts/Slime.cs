using UnityEngine;


public class Slime : MonoBehaviour
{
    private float moveDistance = 50f; // Distancia que se moverá en cada paso
    private float moveSpeed = 0.7f; // Velocidad del movimiento del Slime
    private float pauseDuration = 1f; // Tiempo entre movimientos (en segundos)
    
    // Probabilidades de aparición de items en cada estado
    private enum SlimeState { Speed, GoldHeart, Heart }
    private SlimeState currentState;

    private Vector3 targetPosition; // Posición a la que se moverá
    private Vector3 currentDirection; // Dirección actual del movimiento
    private Vector3 lastDirection; // Última dirección del movimiento
    private float pauseTimer; // Temporizador para manejar pausas
    private RandomNumberValidator randomNumberValidator = new RandomNumberValidator();
    private int score = 3; 
   

    void Start()
    {
        pauseTimer = pauseDuration;
        lastDirection = Vector3.zero;
        SetNewDirection(); // Asignar la primera dirección
        currentState = SlimeState.Heart; // Asignar estado inicial (por ejemplo, Corazón)
    }

    void Update()
    {
        pauseTimer -= Time.deltaTime;

        if (pauseTimer <= 0f)
        {
            // Calcular la nueva dirección y avanzar
            SetNewDirection();
            targetPosition = transform.position + currentDirection * moveDistance;
            pauseTimer = pauseDuration;
        }

        // Mover hacia la posición objetivo
        MoveToTargetPosition();
    }

    void SetNewDirection()
    {
        // Generar una nueva dirección aleatoria diferente a la última
        Vector3 newDirection;
        do
        {
            double randomValue = randomNumberValidator.GetNextNumber();

            if (randomValue < 0.25f)
            {
                newDirection = Vector3.right; // Derecha
            }
            else if (randomValue < 0.5f)
            {
                newDirection = Vector3.up; // Arriba
            }
            else if (randomValue < 0.75f)
            {
                newDirection = Vector3.left; // Izquierda
            }
            else
            {
                newDirection = Vector3.down; // Abajo
            }
        } while (newDirection == lastDirection); // Evitar repetir la misma dirección

        lastDirection = currentDirection; // Actualizar última dirección
        currentDirection = newDirection; // Asignar la nueva dirección
    }

    void MoveToTargetPosition()
    {
        // Moverse gradualmente hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Si llegó a la posición objetivo, esperar hasta que pase el tiempo de pausa
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            targetPosition = transform.position; // Detener el movimiento hasta la próxima dirección
        }
    }

    // Método que simula la caída de un ítem basado en el estado actual
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); 
           
            
        }
        else if (collision.gameObject.CompareTag("Arrow"))
        {
            ScoreText.scoreValue += score;
            Destroy(gameObject); 
            
           
        }
        else if (collision.gameObject.CompareTag("Demon"))
        {
            Debug.Log("Slime chocó con un Demon, pero sigue existiendo.");
        }
        else
        {
            Debug.Log($"Slime colisionó con: {collision.gameObject.name}");
        }
    }
}

