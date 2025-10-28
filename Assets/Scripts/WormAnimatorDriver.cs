// WormSandboxController.cs  (solo para la escena de prueba)
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class WormSandboxController : MonoBehaviour
{
    [Header("Objetivo (dummy)")]
    public Transform target;

    [Header("Movimiento")]
    public float walkSpeed = 1.2f;
    public float runSpeed  = 2.5f;
    public float turnSpeed = 6f;
    public float chaseRange = 8f;
    public float attackRange = 2f;
    public float attackInterval = 1.0f;

    [Header("Tuning animación")]
    public float speedLerp = 8f;      // suaviza el parámetro 'speed'
    public float speedScale = 1f;     // escalar si te queda chico/grande

    Animator anim;
    Rigidbody rb;
    float attackTimer;
    float animSpeed;                  // valor suavizado que mandamos al Animator
    Vector3 desiredVel;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb   = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Controles manuales opcionales para test:
        if (Input.GetKeyDown(KeyCode.Q)) anim.SetTrigger("Attack");
        if (Input.GetKeyDown(KeyCode.E)) anim.SetTrigger("takehit");
        if (Input.GetKeyDown(KeyCode.K)) anim.SetTrigger("death");

        // IA mínima: idle / chase / attack
        Vector3 toTarget = (target ? target.position : transform.position) - transform.position;
        float   dist     = toTarget.magnitude;

        if (target == null || dist > chaseRange)
        {
            // Idle (no nos movemos)
            desiredVel = Vector3.zero;
        }
        else if (dist > attackRange)
        {
            // Perseguir (corre)
            Vector3 dir = toTarget.normalized;
            desiredVel = dir * runSpeed;

            // Girar hacia el movimiento
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * turnSpeed);
            }
        }
        else
        {
            // Atacar
            desiredVel = Vector3.zero;
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                anim.SetTrigger("Attack");
                attackTimer = 0f;
            }
        }

        // Actualizamos 'speed' del Animator en base a la velocidad real
        float v = desiredVel.magnitude;
        animSpeed = Mathf.Lerp(animSpeed, v, Time.deltaTime * speedLerp);
        anim.SetFloat("speed", animSpeed * speedScale);
    }

    void FixedUpdate()
    {
        // Mover por física (sin Root Motion)
        Vector3 newPos = rb.position + desiredVel * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;  Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;     Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
