using UnityEngine;
using UnityEngine.InputSystem;

public class Nave : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource musicaSource;

    public bool EmpujeArriba { get; private set; }
    public bool Izquierda { get; private set; }
    public bool Derecha { get; private set; }

    [Header("Combustible")]
    [SerializeField] private float hidrogeno = 10f;

    [Header("Turbo")]
    [SerializeField] private float velocidadTurbo = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.angularDrag = 3f;
        rb.drag = 0.5f;
    }

    private void Start()
    {
        InputSystem.onAfterUpdate += OnAfterInputUpdate;
        GameObject musica = GameObject.Find("Musica");

        if (musica != null)
        {
            musicaSource = musica.GetComponent<AudioSource>();
        }
    }

    private void OnAfterInputUpdate()
    {
        if (Keyboard.current == null)
        {
            Debug.LogWarning("Keyboard sigue null después de update");
        }
    }

    private void Update()
    {
        
        if (hidrogeno > 0f)
        {
            EmpujeArriba = Input.GetKey(KeyCode.UpArrow);
            Izquierda = Input.GetKey(KeyCode.LeftArrow);
            Derecha = Input.GetKey(KeyCode.RightArrow);
        }
        else
        {
            EmpujeArriba = false;
            Izquierda = false;
            Derecha = false;
        }

        float velocidadActual = rb.velocity.magnitude;


        bool turboActivo = EmpujeArriba && velocidadActual > velocidadTurbo && hidrogeno > 0f;

        animator.SetBool("Turbo", turboActivo);

        Debug.Log("Velocidad: " + velocidadActual);
    }

    private void FixedUpdate()
    {
        float empuje = 300f;
        float torque = 20f;
        float velocidadMax = 5f;

        Debug.Log("Hidrogeno: " + hidrogeno);

        
        if (hidrogeno <= 0f)
        {
            return;
        }

        
        if (EmpujeArriba)
        {
            rb.AddForce(
                (Vector2)transform.up *
                empuje *
                Time.deltaTime
            );

            ConsumoHidrogeno();
        }

        
        if (Izquierda)
        {
            rb.AddTorque(
                torque *
                Time.deltaTime
            );

            ConsumoHidrogeno();
        }
        
        else if (Derecha)
        {
            rb.AddTorque(
                -torque *
                Time.deltaTime
            );

            ConsumoHidrogeno();
        }

        
        rb.velocity =
            Vector2.ClampMagnitude(
                rb.velocity,
                velocidadMax
            );

        
        if (rb.velocity.magnitude >= velocidadTurbo)
        {
            rb.drag = 0.1f;
        }
        else
        {
            rb.drag = 0.5f;
        }
    }

    private void OnCollisionEnter2D(Collision2D choque)
    {
        if (musicaSource != null)
        {
            musicaSource.Pause();
            Invoke(nameof(ReanudarMusica), 2f);
        }
        if (!choque.gameObject.TryGetComponent(out BaseAterrizaje baseAterrizaje))
        {
            Debug.Log("Choque con el Terreno");
            return;
        }

        float inclinacion =
            Vector2.Angle(transform.up, Vector2.up);

        float velocidad =
            Mathf.Abs(choque.relativeVelocity.y);

        int puntaje = 0;

        if (inclinacion < 5f && velocidad < 2f)
        {
            puntaje = 100;
            Debug.Log("Perfecto");
        }
        else if (inclinacion < 10f && velocidad < 4f)
        {
            puntaje = 70;
            Debug.Log("Bueno");
        }
        else if (inclinacion < 15f && velocidad < 6f)
        {
            puntaje = 40;
            Debug.Log("Aceptable");
        }
        else
        {
            Debug.Log("Choque");
            return;
        }

        float puntajeFinal =
            puntaje *
            baseAterrizaje.MultiplicadorPuntaje();

        Debug.Log("Puntaje: " + puntajeFinal);
    }

    private void ConsumoHidrogeno()
    {
        float consumo = 1f;

        hidrogeno -= consumo * Time.deltaTime;

       
        if (hidrogeno < 0f)
        {
            hidrogeno = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (musicaSource != null)
        {
            musicaSource.Pause();
            Invoke(nameof(ReanudarMusica), 2f);
        }
        if (collider2D.TryGetComponent(out Moneda moneda))
        {
            moneda.Recolectar();
        }
    }

    private void ReanudarMusica()
    {
        if (musicaSource != null)
        {
            musicaSource.UnPause();
        }
    }
}