using UnityEngine;
using System.Collections;
using System;
public class Arquero : MonoBehaviour
{
    public GameObject ArrowPrefab;
    public float Speed;
    private Rigidbody2D Rigidbody2D;
    private Vector3 Movement;

    private float Horizontal;
    private float Vertical;

    private Animator Animator;

    private float LastShot;
    private int vida = 6;
    private bool isImmune = false;


    public UIManager uiManager;

    public event EventHandler OnPlayerDeath;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.gravityScale = 0;
        Animator = GetComponent<Animator>();


        // Verifica que el objeto "Game_Manager_UI" existe en la escena
        uiManager = GameObject.Find("GameManagerUI").GetComponent<UIManager>();

        if (uiManager != null)
        {
            uiManager.UpdateLives(vida);
        }

    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-3.0f, 3.0f, 3.0f);
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        }

        Animator.SetBool("isRunning", Horizontal != 0.0f || Vertical != 0.0f);

        Vertical = Input.GetAxisRaw("Vertical");

        bool isRunning = Horizontal != 0.0f || Vertical != 0.0f;

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > LastShot + 0.5f && !isRunning)
        {
            Animator.SetBool("isShooting", true);
            Shot();
            LastShot = Time.time;
        }
        else
        {
            Animator.SetBool("isShooting", false);
        }


    }

    private void Shot()
    {
        Vector3 movement;
        if (transform.localScale.x > 0) // Si el arquero está mirando a la derecha
        {
            movement = Vector3.right;
        }
        else // Si el arquero está mirando a la izquierda
        {
            movement = Vector3.left;
        }

        GameObject arrow = Instantiate(ArrowPrefab, transform.position + movement * 0.3f + new Vector3(0, 0.1f, 0), Quaternion.identity);
        arrow.GetComponent<Arrow>().setMovement(movement);
    }

    void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * Speed, Vertical * Speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Demon") && !isImmune)
        {
            StartCoroutine(TakeDamage());
            vida--;
            uiManager.UpdateLives(vida);
            GetComponent<SpriteRenderer>().color = Color.red;


        }


    }



    private IEnumerator TakeDamage()
    {


        if (vida <= 1)
        {
            Disappear();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            isImmune = true; // Activar inmunidad
            yield return new WaitForSeconds(1); // Esperar 1 segundo
            isImmune = false; // Desactivar inmunidad
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Disappear()
    {
        // Aquí puedes agregar un efecto de desaparición si lo deseas
        gameObject.SetActive(false); // Desactiva el objeto arquero
        Debug.Log("Arquero has disappeared.");
    }



}

