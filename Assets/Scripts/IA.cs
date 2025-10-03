using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    GameObject[] ruta1;
    [SerializeField] GameObject CameraRig;
    [SerializeField] float speed = 2f; // Puedes subir la velocidad si lo ves lento
    [SerializeField] float distanceChangePoint = 1f;
    [SerializeField] float rotspeed = 5f;
    [SerializeField] GameObject Corona;
    Rigidbody rb;
    int currentpoint = 0;
    float range = 500f;

    public enum EnemyState { PATRULLAR, ATACAR }
    [SerializeField] public EnemyState enemyState;

    private void Start()
    {
        ruta1 = GameObject.FindGameObjectsWithTag("point");
        enemyState = EnemyState.PATRULLAR;
        rb = Corona.GetComponent<Rigidbody>();
    }

    private void Patrol()
    {
        if (ruta1.Length == 0 || Corona == null || rb == null) return;
        if (ruta1[currentpoint] == null) return;
        Vector3 target = ruta1[currentpoint].transform.position;
        Vector3 moveDirection = (target - Corona.transform.position).normalized;

        Corona.transform.rotation = Quaternion.Slerp(
            Corona.transform.rotation,
            Quaternion.LookRotation(moveDirection),
            rotspeed * Time.deltaTime
        );

        if ((target - Corona.transform.position).magnitude < distanceChangePoint)
        {
            currentpoint++;
            if (currentpoint >= ruta1.Length)
            {
                currentpoint = 0;
            }
        }
        rb.MovePosition(Corona.transform.position + moveDirection * speed * Time.deltaTime);
    }

private void Attack()
{
    if (CameraRig == null || Corona == null) return;

    Vector3 target = CameraRig.transform.position;
    Vector3 moveDirection = (target - Corona.transform.position).normalized;

    // Girar hacia el jugador
    Corona.transform.rotation = Quaternion.Slerp(
        Corona.transform.rotation,
        Quaternion.LookRotation(moveDirection),
        rotspeed * Time.deltaTime
    );

    float distance = Vector3.Distance(Corona.transform.position, target);

    if (distance > 1.5f) // 🔹 si está lejos, acercarse
    {
        rb.MovePosition(Corona.transform.position + moveDirection * speed * Time.deltaTime);
    }
    else
    {
        // 🔹 si ya está suficientemente cerca, frenar
        rb.velocity = Vector3.zero; 
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