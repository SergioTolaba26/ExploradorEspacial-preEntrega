using UnityEngine;

public class BaseAterrizaje : MonoBehaviour
{
    [SerializeField] private int multiplicadorPuntaje;

    public int MultiplicadorPuntaje()
    {
        return multiplicadorPuntaje;
    }
}
