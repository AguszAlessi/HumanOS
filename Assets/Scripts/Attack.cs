using UnityEngine;
using UnityEngine.UI;

public class AttackPlayer : MonoBehaviour
{
    public IA ia;
    public PlayerLife playerLife;
    public Transform player;
    public float damageAmount = 10f;
    public float attackInterval = 1f;
    public float attackRange = 2f;
    public AudioClip hitSound;

    private AudioSource audioSource;
    private float attackTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (ia.enemyState == IA.EnemyState.ATACAR && player != null && playerLife != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackInterval)
                {
                    playerLife.TakeDamage(damageAmount);

                    if (hitSound != null)
                        audioSource.PlayOneShot(hitSound);

                    attackTimer = 0f;
                }
            }
            else
            {
                attackTimer = 0f;
            }
        }
    }
}
