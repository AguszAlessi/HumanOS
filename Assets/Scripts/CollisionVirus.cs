// CollisionVirus.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollisionVirus : MonoBehaviour
{
    public GameObject virus;
    public GameObject victoryPanel; // Panel con blur y mensaje

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
            Destroy(virus);
            SceneManager.LoadScene("Victory");
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}