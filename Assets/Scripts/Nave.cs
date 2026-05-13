using UnityEngine;
using UnityEngine.InputSystem;


public class Nave : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.4f;

    private Rigidbody2D rb;
    public bool EmpujeArriba { get; private set; }
    public bool Izquierda { get; private set; }
    public bool Derecha { get; private set; }

    public static Nave Instancia { get; private set; }

    private float hidrogeno = 10f;

    public enum State
    {
        Volando,
        Aterrizando,
        Estrellado
    }
    private State estado = State.Volando;


    private void Awake()
    {
        Instancia = this;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        rb.angularDrag = 3f;
        rb.drag = 0.5f;
    }

    private void FixedUpdate()
    {
        float empuje = 300f;
        float torque = 20f;
        float velocidadMax = 5f;



        EmpujeArriba = Keyboard.current.upArrowKey.isPressed;
        Izquierda = Keyboard.current.leftArrowKey.isPressed;
        Derecha = Keyboard.current.rightArrowKey.isPressed;

        //Debug.Log(hidrogeno);
        if (hidrogeno <= 0f)
        {
            EmpujeArriba = false;
            Izquierda = false;
            Derecha = false;
            return;
        }

        if (EmpujeArriba)
        {
            rb.AddForce((Vector2)transform.up * empuje * Time.deltaTime);
            ConsumoHidrogeno();
            rb.gravityScale = GRAVITY_NORMAL;
        }

        if (Izquierda)
        {
            rb.AddTorque(torque * Time.deltaTime);
            ConsumoHidrogeno();
        }
        else if (Derecha)
        {
            rb.AddTorque(-torque * Time.deltaTime);
            ConsumoHidrogeno();
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, velocidadMax);
    }


    private void OnCollisionEnter2D(Collision2D choque)
    {
        if (!choque.gameObject.TryGetComponent(out BaseAterrizaje baseAterrizaje))
        {
            Debug.Log("Choque con el Terreno");
            return;
        }

        float inclinacion = Vector2.Angle(transform.up, Vector2.up);
        float velocidad = Mathf.Abs(choque.relativeVelocity.y);

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

        int puntajeFinal = Mathf.RoundToInt(puntaje * baseAterrizaje.MultiplicadorPuntaje());
        Puntuacion.Instancia.SumarPuntos(puntajeFinal);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        Debug.Log("Trigger con: " + collider2D.name);

        if (collider2D.TryGetComponent(out CargaH2 cargaH2))
        {
            Debug.Log("Destruyendo H2");

            hidrogeno += 5f;
            Puntuacion.Instancia.SumarPuntos(5);
            cargaH2.Destruite();
        }
    
        if (collider2D.gameObject.TryGetComponent(out Moneda moneda))
        {
            Puntuacion.Instancia.SumarPuntos(10);
            moneda.Recolectar();
        }

    }
    private void ConsumoHidrogeno()
    {
        float consumo = 1f;
        hidrogeno -= consumo * Time.deltaTime;
        Debug.Log("Hidrogeno: " + hidrogeno);
    }

    public float ObtenerHidrogeno()
    {
        return hidrogeno;
    }
    public float ObtenerVelocidadX()
    {
        return rb.velocity.x;
    }
    public float ObtenerVelocidadY()
    {
        return rb.velocity.y;
    }
}