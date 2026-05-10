using UnityEngine;

public class NaveVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem fuegoIzquierdo;
    [SerializeField] private ParticleSystem fuegoCentral;
    [SerializeField] private ParticleSystem fuegoDerecho;

    private Nave nave;

    private ParticleSystem.EmissionModule emisionIzquierda;
    private ParticleSystem.EmissionModule emisionCentral;
    private ParticleSystem.EmissionModule emisionDerecha;

    private void Awake()
    {
        nave = GetComponent<Nave>();

        
        emisionIzquierda = fuegoIzquierdo.emission;
        emisionCentral = fuegoCentral.emission;
        emisionDerecha = fuegoDerecho.emission;

        
        emisionIzquierda.enabled = false;
        emisionCentral.enabled = false;
        emisionDerecha.enabled = false;

       
        fuegoIzquierdo.Play();
        fuegoCentral.Play();
        fuegoDerecho.Play();
    }

    private void Update()
    {
        
        if (nave.EmpujeArriba)
        {
            emisionIzquierda.enabled = true;
            emisionCentral.enabled = true;
            emisionDerecha.enabled = true;
        }
        else
        {
            emisionCentral.enabled = false;
        }

        
        if (nave.Izquierda)
        {
            emisionDerecha.enabled = true;
        }
        else if (!nave.EmpujeArriba)
        {
            emisionDerecha.enabled = false;
        }

        
        if (nave.Derecha)
        {
            emisionIzquierda.enabled = true;
        }
        else if (!nave.EmpujeArriba)
        {
            emisionIzquierda.enabled = false;
        }
    }
}