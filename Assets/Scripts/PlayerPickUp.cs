using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MyInteraction : MonoBehaviour
{
    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        Debug.Log("¡Objeto clickeado con el gatillo!");
        // Ejemplo: cambiar color al azar
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
