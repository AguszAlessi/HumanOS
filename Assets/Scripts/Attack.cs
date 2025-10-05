using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : MonoBehaviour
{
    public IA ia;
    public Slider playerHealthBar;
    public float damageAmount = 10f;
    public float attackInterval = 1f;
    public float attackRange = 2f; // Distancia para atacar al jugador
    public Transform player;
    public AudioClip hitSound;
    private AudioSource audioSource;
    private float attackTimer = 0f;

   void Start()
    {

    audioSource = GetComponent<AudioSource>();
    
    }
    void Update()
    {
        if (ia.enemyState == IA.EnemyState.ATACAR && player != null && playerHealthBar != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    playerHealthBar.value -= damageAmount;
                    attackTimer = 0f;
                     if (hitSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(hitSound);
                    }

                }
            }
            else
            {
                attackTimer = 0f;
            }
        }
    }
}