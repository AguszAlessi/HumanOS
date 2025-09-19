using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusInfoPanel : MonoBehaviour
{
    public GameObject infoPanel; // Asigna el panel en el inspector

    void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false); // Panel desactivado al inicio
    }

    public void ShowPanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(true); // Activa el panel cuando se destruye el virus
    }
}
