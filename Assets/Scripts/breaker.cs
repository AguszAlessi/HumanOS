using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Breaker : MonoBehaviour
{
    public Rigidbody rb; 
    public bool isBroken;

    void Start()
    {
        rb.isKinematic = true;
    }

    void Update()
    {
      if(isBroken==true)
      {
        rb.isKinematic = false;
      }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Breaker")
        {
            isBroken = true;

            
        }
    }
}