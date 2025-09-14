using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : MonoBehaviour
{
    GameObject[] ruta1; 
    [SerializeField] GameObject CameraRig;
    [SerializeField] float speed = 0.01f;
    [SerializeField] float distanceChangePoint = 1f;
    [SerializeField] float rotspeed = 5f;
    [SerializeField] GameObject Enemigo;
    int currentpoint = 0;
    float range = 500f;

    public enum EnemyState { PATRULLAR, ATACAR } // Enum debe ser público y fuera de SerializeField
    [SerializeField] public EnemyState enemyState; // Usa el mismo nombre

    private void Start()
    {
        ruta1 = GameObject.FindGameObjectsWithTag("point");
        enemyState = EnemyState.PATRULLAR;
    }
    private void Patrol()
    {
        if(ruta1.Length == 0) return;
        Vector3 target = new Vector3(ruta1[currentpoint].transform.position.x, ruta1[currentpoint].transform.position.y, ruta1[currentpoint].transform.position.z);

        Vector3 moveDirection = target - Enemigo.transform.position;

        Enemigo.transform.rotation = Quaternion.Slerp(Enemigo.transform.rotation, Quaternion.LookRotation(moveDirection), rotspeed * Time.deltaTime);

        if(moveDirection.magnitude < distanceChangePoint)
        {
            currentpoint++;
            if(currentpoint >= ruta1.Length)
            {
                currentpoint = 0;
            }
        }
        Enemigo.transform.Translate(0, 0, speed* Time.deltaTime);      
    }

     private void Attack()
    {
        Vector3 target = new Vector3(CameraRig.transform.position.x, Enemigo.transform.position.y, CameraRig.transform.position.z);
        Vector3 moveDirection = target - Enemigo.transform.position;
        Enemigo.transform.rotation = Quaternion.Slerp(Enemigo.transform.rotation, Quaternion.LookRotation(moveDirection), rotspeed * Time.deltaTime);
        Enemigo.transform.Translate(0, 0, speed * Time.deltaTime);


    }

    
    private void EnemyStateFunction()
    {
        switch (enemyState) // Usa el mismo nombre
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
    private void Update()
    {
        EnemyStateFunction();
    }

}
