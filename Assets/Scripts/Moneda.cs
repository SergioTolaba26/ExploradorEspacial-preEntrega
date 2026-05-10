using UnityEngine;

public class Moneda : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private AudioClip sonidoMoneda;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Recolectar()
    {
        
        AudioSource.PlayClipAtPoint(
            sonidoMoneda,
            transform.position
        );

        
        animator.SetTrigger("Recolectada");

        
        Destroy(gameObject, 0.5f);
    }
}