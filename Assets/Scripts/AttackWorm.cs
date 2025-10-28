// AttackWorm.cs (versión simple con Animator)
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackWorm : MonoBehaviour
{
    [Header("Refs")]
    public IAWorm ia;                 // IA del worm
    public Transform player;          // si no se asigna, busca tag "Player"
    public PlayerLife playerLife;     // script de vida del player (si existe)

    [Header("Ataque")]
    public float damageAmount = 10f;
    public float attackInterval = 1f;
    public float attackRange = 2f;
    public AudioClip hitSound;

    // internos
    AudioSource audioSource;
    float attackTimer = 0f;

    // animación
    Animator animator;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (ia == null) ia = GetComponentInParent<IAWorm>() ?? GetComponent<IAWorm>();

        if (player == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }

        animator = GetComponentInChildren<Animator>() ?? GetComponent<Animator>();
    }

    void Update()
    {
        if (ia == null || player == null || playerLife == null) return;

        // sólo ataca si IA está en ATACAR
        if (ia.enemyState != IAWorm.EnemyState.ATACAR) { attackTimer = 0f; return; }

        // posición del cuerpo para medir distancia
        Transform bodyT = ia.GetBody() != null ? ia.GetBody().transform : transform;

        float distance = Vector3.Distance(bodyT.position, player.position);
        if (distance > attackRange) { attackTimer = 0f; return; }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            // daño
            playerLife.TakeDamage(damageAmount);

            // anim
            if (animator != null) animator.SetTrigger("Attack");

            // audio
            if (hitSound != null) audioSource.PlayOneShot(hitSound);

            attackTimer = 0f;
        }
    }
}
