using UnityEngine;

public class NaveAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioEmpuje;

    private Nave nave;

    private void Awake()
    {
        nave = GetComponent<Nave>();
    }

    private void Start()
    {
        audioEmpuje.enabled = true; 
    }

    private void Update()
    {
        if (nave == null) return;

        bool motorActivo = nave.EmpujeArriba || nave.Izquierda || nave.Derecha;

        if (motorActivo)
        {
            if (!audioEmpuje.isPlaying)
            {
                audioEmpuje.Play();
            }
        }
        else
        {
            if (audioEmpuje.isPlaying)
            {
                audioEmpuje.Stop();
            }
        }
    }

}