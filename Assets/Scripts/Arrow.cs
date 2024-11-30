using UnityEngine;


public class Arrow : MonoBehaviour
{

    private Rigidbody2D Rigidbody2D;
    private Vector3 Movement;  
    
    

    public float Speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
       

    }  



    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = Movement * Speed; ///Cam
    }

    public void setMovement(Vector3 movement)
    {
        Movement = movement;
        if (movement == Vector3.right)
        {
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }
        else
        {
            transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si colisiona con un objeto que tenga el tag "Wall" o cualquier otro tag que desees
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject); // Destruye la flecha
        }
        ///
        else if (collision.gameObject.CompareTag("Demon")) // Verifica si colisiona con el demonio
        {
            // Llama al método de daño en el demonio
            collision.gameObject.GetComponent<DemonBehavior>().TakeDamage();
            Destroy(gameObject); // Destruye la flecha al impactar
        }

        else if(collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("Speed")) // Verifica si colisiona con el slime
        {
            Destroy(gameObject); // Destruye la flecha al impactar
        }
    }


}
