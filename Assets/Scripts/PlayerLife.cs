using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [Header("Salud")]
    public Image healthCircle;
    public float maxHealth = 100f;
    [HideInInspector] public float currentHealth;

    [Header("Colores")]
    public Color fullColor = Color.green;
    public Color lowColor = Color.red;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateCircle();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateCircle();

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("Defeat");
        }
    }

    private void UpdateCircle()
    {
        if (healthCircle != null)
        {
            float fill = currentHealth / maxHealth;
            healthCircle.fillAmount = fill;
            healthCircle.color = Color.Lerp(lowColor, fullColor, fill);
        }
    }
}
