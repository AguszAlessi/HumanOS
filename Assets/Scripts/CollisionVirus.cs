// CollisionVirus.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CollisionVirus : MonoBehaviour
{
    public GameObject virus;
    public GameObject victoryPanel; // Panel con blur y mensaje
    public Slider virusHealthBar;   
    public float damageAmount = 10f;


    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

 private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Antivirus")
        {
            if (virusHealthBar != null)
            {
                virusHealthBar.value -= damageAmount;
                
                if (virusHealthBar.value <= 0)
                {
                    // Busca el hijo CanvasPanelVariant y lo separa del virus
                    Transform panel = transform.Find("CanvasPanel Variant");
                    if (panel != null)
                    {
                        panel.SetParent(null);
                        panel.gameObject.SetActive(true);
                    }
                    Destroy(virus);
                    Console.WriteLine("choque");
                }
            }
        }
    }

    public void GoToMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
    }
}