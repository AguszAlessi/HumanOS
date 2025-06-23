using UnityEngine;
using UnityEngine.SceneManagement; 

public class CollisionVirus : MonoBehaviour
{
    public GameObject virus;
    public float fadeDuration = 1.0f; // Duración del desvanecimiento

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "antivirus")
        {
            Destroy(virus);
            SceneManager.LoadScene("Victory"); // Cambia a la escena Victory
        }
    }
}