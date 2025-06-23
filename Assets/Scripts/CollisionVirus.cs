using UnityEngine;

public class CollisionVirus : MonoBehaviour
{
    public GameObject victoryPanel;
    public float fadeDuration = 1.0f; // Duración del desvanecimiento

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Virus"))
        {
            if (victoryPanel != null)
            {
                victoryPanel.SetActive(true);
            }

            StartCoroutine(FadeAndDestroy(collision.gameObject));
        }
    }

    private System.Collections.IEnumerator FadeAndDestroy(GameObject virus)
    {
        Renderer rend = virus.GetComponent<Renderer>();
        if (rend == null)
        {
            Destroy(virus);
            yield break;
        }

        Material mat = rend.material;
        if (!mat.HasProperty("_Color"))
        {
            Destroy(virus);
            yield break;
        }

        // Asegurate que el material esté en modo transparente o fade
        Color originalColor = mat.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            mat.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(virus);
    }
}