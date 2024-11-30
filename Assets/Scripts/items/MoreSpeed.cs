using UnityEngine;

public class MoreSpeed : MonoBehaviour
{
    public float timeToDisappear = 10f; // Tiempo de vida del objeto en segundos
    public float speedBoostAmount = 2f; // Cantidad de velocidad adicional
    public float speedBoostDuration = 5f; // Duración del aumento de velocidad en segundos
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
            Debug.Log("MoreSpeed ha desaparecido por no ser recogido.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador recoge el objeto de velocidad
        if (collision.gameObject.CompareTag("Player"))
        {
            
        }
        // Si una flecha impacta el objeto
        else if (collision.gameObject.CompareTag("Arrow"))
        {
            Destroy(collision.gameObject); // Destruye la flecha
            Destroy(gameObject); // Destruye el objeto
            Debug.Log("Una flecha impactó MoreSpeed y lo destruyó.");
        }
    }

   
}
