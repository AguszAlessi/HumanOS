using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] IA ia;
   
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                ia.enemyState = IA.EnemyState.ATACAR;

            }
        }
    
}
