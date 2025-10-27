using UnityEngine;

public class ShowPanelInfo : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false); // Desactiva el propio objeto al inicio
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true); // Activa el propio objeto cuando se llama
    }
}