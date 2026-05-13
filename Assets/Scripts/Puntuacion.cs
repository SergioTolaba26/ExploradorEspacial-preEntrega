using UnityEngine;

public class Puntuacion : MonoBehaviour
{
    public static Puntuacion Instancia;

    private int puntajeTotal = 0;
    private float tiempo;

    private void Awake()
    {
        // Singleton simple
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SumarPuntos(int puntos)
    {
        puntajeTotal += puntos;
        Debug.Log("Puntaje total: " + puntajeTotal);
    }

    public int ObtenerPuntaje()
    {
        return puntajeTotal;
    }
    private void Update()
    {
        tiempo += Time.deltaTime;
    }
    public float ObtenerTiempo()
    {
        return Mathf.RoundToInt(tiempo);
    }
}