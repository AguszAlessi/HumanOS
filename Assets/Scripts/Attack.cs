// AttackPlayer.cs (tu versión + animación)
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
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

    // NUEVO


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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

                    if (hitSound != null) audioSource.PlayOneShot(hitSound);

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
