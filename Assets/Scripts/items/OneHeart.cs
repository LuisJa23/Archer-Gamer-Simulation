using UnityEngine;

public class OneHeart : MonoBehaviour
{
    public float timeToDisappear = 10f; // Tiempo de vida del corazón en segundos
    private float timer;

    void Start()
    {
        timer = timeToDisappear;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Destroy(gameObject);
            Debug.Log("OneHeart ha desaparecido por no ser recogido.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador recoge el corazón
        if (collision.gameObject.CompareTag("Player"))
        {
            AddHeartToPlayer(collision.gameObject);
        }
        // Si una flecha impacta el corazón
        else if (collision.gameObject.CompareTag("Arrow"))
        {
            Arquero arquero = GameObject.FindWithTag("Player")?.GetComponent<Arquero>();
            if (arquero != null)
            {
                arquero.AddLife(1);
                Debug.Log("Una flecha impactó el corazón. Vida del jugador aumentada.");
            }

            Destroy(collision.gameObject); // Destruye la flecha
            Destroy(gameObject); // Destruye el corazón
        }
    }

    private void AddHeartToPlayer(GameObject player)
    {
        Arquero arquero = player.GetComponent<Arquero>();
        if (arquero != null)
        {
            arquero.AddLife(1); // Aumenta la vida del jugador
            Destroy(gameObject); // Destruye el corazón
            Debug.Log("El jugador ha recogido el OneHeart y ha ganado 1 vida.");
        }
    }
}
