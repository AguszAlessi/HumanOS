// IAWorm.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAWorm : MonoBehaviour
{
    // === Referencias ===
    [Header("Referencias")]
    [SerializeField] Transform CameraRig;     // objetivo a perseguir (tu player/cámara)
    [SerializeField] GameObject Corona;       // cuerpo que se mueve (si está vacío, usa este GO)

    // === Movimiento ===
    [Header("Movimiento")]
    [SerializeField] float speed = 2f;
    [SerializeField] float distanceChangePoint = 1f;
    [SerializeField] float rotspeed = 5f;

    // === Patrulla ===
    [Header("Patrulla")]
    [SerializeField] string patrolPointTag = "point";
    GameObject[] ruta1;
    int currentpoint = 0;

    // === Estados ===
    public enum EnemyState { PATRULLAR, ATACAR }
    [SerializeField] public EnemyState enemyState = EnemyState.PATRULLAR;

    // === Ataque / persecución ===
    [Header("Rangos")]
    [SerializeField] float chaseRange = 6f;     // cambia a ATACAR si el player entra aquí
    [SerializeField] float stopDistance = 1.5f; // distancia mínima al player

    // === Internos ===
    Rigidbody rb;

    void Awake()
    {
        // Fallbacks: si no se asigna, usa este mismo GO
        if (Corona == null) Corona = this.gameObject;

        // Rigidbody del cuerpo
        rb = Corona.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = Corona.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        // Si no asignaron CameraRig, intentamos buscar un "Player"
        if (CameraRig == null)
        {
            var p = GameObject.FindWithTag("Player");
            if (p != null) CameraRig = p.transform;
        }
    }

    void Start()
    {
        ruta1 = GameObject.FindGameObjectsWithTag(patrolPointTag);
        enemyState = EnemyState.PATRULLAR;
    }

    void Update()
    {
        // Cambio de estado por distancia al player (si existe)
        if (CameraRig != null)
        {
            float d = Vector3.Distance(Corona.transform.position, CameraRig.position);
            enemyState = (d <= chaseRange) ? EnemyState.ATACAR : EnemyState.PATRULLAR;
        }

        // Ejecutar estado
        if (enemyState == EnemyState.PATRULLAR) Patrol();
        else Attack();
    }

    void Patrol()
    {
        if (ruta1 == null || ruta1.Length == 0 || ruta1[currentpoint] == null || rb == null) return;

        Vector3 target = ruta1[currentpoint].transform.position;
        Vector3 dir = (target - Corona.transform.position).normalized;

        // girar
        Corona.transform.rotation = Quaternion.Slerp(
            Corona.transform.rotation,
            Quaternion.LookRotation(dir),
            rotspeed * Time.deltaTime
        );

        // mover
        rb.MovePosition(Corona.transform.position + dir * speed * Time.deltaTime);

        // cambiar punto
        if ((target - Corona.transform.position).magnitude < distanceChangePoint)
        {
            currentpoint = (currentpoint + 1) % ruta1.Length;
        }
    }

    void Attack()
    {
        if (CameraRig == null || rb == null) return;

        Vector3 target = CameraRig.position;
        Vector3 dir = (target - Corona.transform.position).normalized;

        Corona.transform.rotation = Quaternion.Slerp(
            Corona.transform.rotation,
            Quaternion.LookRotation(dir),
            rotspeed * Time.deltaTime
        );

        float distance = Vector3.Distance(Corona.transform.position, target);
        if (distance > stopDistance)
            rb.MovePosition(Corona.transform.position + dir * speed * Time.deltaTime);
        else
            rb.velocity = Vector3.zero;
    }

    // Getters para asignar desde otros scripts si querés
    public Transform GetCameraRig() => CameraRig;
    public GameObject GetBody() => Corona;
}
