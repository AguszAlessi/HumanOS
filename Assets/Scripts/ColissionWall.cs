using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionWall : MonoBehaviour
{
    public GameObject virus;
    public float fadeDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VirusCollision"))
        {
            Destroy(virus);
            SceneManager.LoadScene("Victory");
        }
    }
}