using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public Slider healthBar;
    public float damageAmount = 10f;
    public float damageInterval = 1f; // Tiempo entre daños en segundos

    private float damageTimer = 0f;

void OnCollisionStay(Collision collision)
{
    if (collision.gameObject.CompareTag("Virus"))
    {
        Debug.Log("Jugador está en contacto con el virus");

        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            if (healthBar != null)
            {
                healthBar.value -= damageAmount;
                Debug.Log("Vida actual: " + healthBar.value);

                if (healthBar.value <= 0)
                {
                    SceneManager.LoadScene("Defeat");
                }
            }

            damageTimer = 0f;
        }
    }
}


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Virus"))
        {
            damageTimer = 0f; // reset al separarse
        }
    }
}
