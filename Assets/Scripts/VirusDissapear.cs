using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusDissapear : MonoBehaviour
{
    public GameObject Virus;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Antivirus"))
        {
            Destroy(Virus);
        }
    }
}
