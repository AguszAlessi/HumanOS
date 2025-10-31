using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    GameObject[] ruta1;
    public Animator animator;
    [SerializeField] GameObject CameraRig;
    [SerializeField] float speed = 2f; // Puedes subir la velocidad si lo ves lento
    [SerializeField] float distanceChangePoint = 1f;
    [SerializeField] float rotspeed = 5f;
    // Suavizado exponencial para rotación (opción B)
    [SerializeField] float rotSmooth = 10f; // cuanto mayor, más rápido responde
    [SerializeField] GameObject Corona;
    Rigidbody rb;
    int currentpoint = 0;
    float range = 500f;

    public enum EnemyState { PATRULLAR, ATACAR }
    [SerializeField] public EnemyState enemyState;
    
    Vector3 lastPos;
    private void Awake()
    {
        // intentar obtener Animator del mismo objeto o de hijos
        animator = animator != null ? animator : GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        ruta1 = GameObject.FindGameObjectsWithTag("point");
        enemyState = EnemyState.PATRULLAR;
        if (Corona != null)
            rb = Corona.GetComponent<Rigidbody>();
    }

    // helper seguro para setear animaciones
    private void SafeSetBool(string name, bool value)
    {
        if (animator != null)
            animator.SetBool(name, value);
    }

    private void Patrol()
    {
        SafeSetBool("Walk", true);
        SafeSetBool("Run", false);
        SafeSetBool("Attack", false);

        if (ruta1 == null || ruta1.Length == 0 || Corona == null || rb == null) return;
        if (ruta1[currentpoint] == null) return;
        Vector3 target = ruta1[currentpoint].transform.position;
        Vector3 moveDirection = (target - Corona.transform.position).normalized;

        // Aplicar rotación al Rigidbody (suavizado exponencial)
        Quaternion targetRot = Quaternion.LookRotation(moveDirection);
        float t = 1f - Mathf.Exp(-rotSmooth * Time.deltaTime);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, t));

        // Mover hacia delante según la rotación actual (asegura que "adelante" sea local Z)
        Vector3 forward = rb.rotation * Vector3.forward;
        rb.MovePosition(rb.position + forward * speed * Time.deltaTime);

        if ((target - Corona.transform.position).magnitude < distanceChangePoint)
        {
            currentpoint++;
            if (currentpoint >= ruta1.Length)
            {
                currentpoint = 0;
            }
        }
    }

private void Attack()
{
    if (CameraRig == null || Corona == null || rb == null) return;

    Vector3 target = CameraRig.transform.position;
    Vector3 moveDirection = (target - Corona.transform.position).normalized;

    // Rotar el Rigidbody hacia el jugador (suavizado exponencial)
    Quaternion targetRot = Quaternion.LookRotation(moveDirection);
    float t = 1f - Mathf.Exp(-rotSmooth * Time.deltaTime);
    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, t));

    float distance = Vector3.Distance(Corona.transform.position, target);

    if (distance > 1.5f) // si está lejos, acercarse
    {
        SafeSetBool("Walk", false);
        SafeSetBool("Run", true);
        SafeSetBool("Attack", false);

        // mover hacia adelante según la rotación aplicada
        Vector3 forward = rb.rotation * Vector3.forward;
        rb.MovePosition(rb.position + forward * speed * Time.deltaTime);
    }
    else
    {
        rb.velocity = Vector3.zero; 
        SafeSetBool("Walk", false);
        SafeSetBool("Run", false);
        SafeSetBool("Attack", true);
    }
}

private void Update()
{
    if (Corona == null || CameraRig == null) return; // 👈 chequeo antes de acceder

    float distanceToPlayer = Vector3.Distance(Corona.transform.position, CameraRig.transform.position);

    if (distanceToPlayer <= 6f) 
    {
        enemyState = EnemyState.ATACAR;
    }
    else
    {
        enemyState = EnemyState.PATRULLAR;
    }

    EnemyStateFunction();
}



    private void EnemyStateFunction()
    {
        switch (enemyState)
        {
            case EnemyState.PATRULLAR:
                Patrol();
                break;
            case EnemyState.ATACAR:
                Attack();
                break;
            default:
                Patrol();
                break;
        }
    }

}