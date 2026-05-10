
using TMPro;
using UnityEngine;

public class BaseAterrizajeVista : MonoBehaviour
{

    [SerializeField] private TextMeshPro MultiplicadorPuntajeText;

    private void Awake()
    {
        BaseAterrizaje baseAterrizaje = GetComponent<BaseAterrizaje>();
        MultiplicadorPuntajeText.text = "x" + baseAterrizaje.MultiplicadorPuntaje();
    }


}
