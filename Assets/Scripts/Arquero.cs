using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Audio;

public class Arquero : MonoBehaviour
{
    public GameObject ArrowPrefab;
    public float Speed;
    private Rigidbody2D Rigidbody2D;

    private float Horizontal;
    private float Vertical;

    private Animator Animator;

    private float LastShot;
    private int vida = 6; // Vida inicial
    private bool isImmune = false;
    private bool isSpeedBoosted = false;

    public UIManager uiManager;

    public event EventHandler OnPlayerDeath;

    private SpriteRenderer spriteRenderer;
    private Color baseColor;

    public AudioSource audioSource;
    public AudioSource powerUpSound;
    
    public AudioSource damageSound;
    public AudioSource killSound;

    private float timeSinceLastDemonKill = 0f; // Tiempo desde el último Demon eliminado


    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Rigidbody2D.gravityScale = 0;
        Animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;

        uiManager = GameObject.Find("GameManagerUI").GetComponent<UIManager>();

        if (uiManager != null)
        {
            uiManager.UpdateLives(vida);
        }

        // Iniciar la corrutina para verificar daño por inactividad
        StartCoroutine(CheckNoKillPenalty());
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
        audioSource.Play();
        Vector3 movement = transform.localScale.x > 0 ? Vector3.right : Vector3.left;

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
            StartCoroutine(ApplyEnemyDamage());
            OnDemonKilled(); // Reiniciar el tiempo al matar un Demon
        }

        if (collision.gameObject.CompareTag("Slime") && !isImmune)
        {
            StartCoroutine(ApplyEnemyDamage());
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            AddLife(2); // Incrementa la vida en 2
            Destroy(collision.gameObject); // Destruye el objeto del ítem
        }

        if (collision.gameObject.CompareTag("Speed") && !isSpeedBoosted)
        {
            Destroy(collision.gameObject); // Destruye el objeto Speed
            StartCoroutine(ApplySpeedBoost());
        }
    }

    private IEnumerator ApplySpeedBoost()
    {
        powerUpSound.Play();
        isSpeedBoosted = true;
        float originalSpeed = Speed;
        Speed *= 2; // Duplica la velocidad
        Debug.Log("Velocidad aumentada temporalmente.");

        float elapsedTime = 0f;

        while (elapsedTime < 7f) // Aumenta la velocidad por 7 segundos
        {
            UpdateSpriteColor(isImmune ? Color.red : Color.yellow);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restaurar la velocidad y el color original
        Speed = originalSpeed;
        isSpeedBoosted = false;
        UpdateSpriteColor(isImmune ? Color.red : baseColor);
        Debug.Log("El aumento de velocidad ha terminado.");
    }

    public IEnumerator ApplyEnemyDamage()
    {
        if (vida > 1)
        {
            damageSound.Play();
            vida--;
            uiManager.UpdateLives(vida);
            Debug.Log("Daño recibido. Vida actual: " + vida);

            isImmune = true; // Activar inmunidad temporal
            UpdateSpriteColor(Color.red);

            yield return new WaitForSeconds(1); // Inmunidad por 1 segundo

            isImmune = false; // Desactivar inmunidad
            UpdateSpriteColor(isSpeedBoosted ? Color.yellow : baseColor);
        }
        else
        {
            Disappear();
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateSpriteColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private void Disappear()
{
    gameObject.SetActive(false); // Desactiva el objeto arquero
    Debug.Log("Arquero ha desaparecido.");
}




    public void AddLife(int amount)
    {
        powerUpSound.Play();
        vida = Mathf.Min(vida + amount, 6); // Asegúrate de que la vida no exceda el máximo de 6
        uiManager.UpdateLives(vida);
        Debug.Log("Vida aumentada. Nueva vida: " + vida);
    }

    private IEnumerator CheckNoKillPenalty()
    {
        while (vida > 0) // Continuar verificando mientras el arquero esté vivo
        {
            timeSinceLastDemonKill += Time.deltaTime;

            if (timeSinceLastDemonKill >= 7f) // Si han pasado 7 segundos sin matar un Demon
            {
                Debug.LogWarning("El arquero no ha matado a ningún Demon en 7 segundos. Recibiendo daño.");
                StartCoroutine(ApplyEnemyDamage()); // Aplicar daño por inactividad
                timeSinceLastDemonKill = 0f; // Reiniciar el contador
            }

            yield return null; // Esperar al siguiente frame
        }
    }

    public void OnDemonKilled()
    {
        killSound.Play(); // Reproducir sonido de matar
        timeSinceLastDemonKill = 0f; // Reiniciar el contador
        Debug.Log("Demon eliminado. Tiempo sin matar reiniciado.");
    }

    
}
