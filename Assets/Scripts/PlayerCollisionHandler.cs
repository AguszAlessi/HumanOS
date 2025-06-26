// Revisado para que cualquier colisión detenga el movimiento
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCollisionHandler : MonoBehaviour
{
    private Rigidbody rb;
    private bool isColliding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision detectada con: " + collision.gameObject.name);
        isColliding = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }

    void FixedUpdate()
    {
        if (isColliding)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
