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

    private float attackTimer = 0f;

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
                }
            }
            else
            {
                attackTimer = 0f;
            }
        }
    }
}