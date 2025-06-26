using UnityEngine;

public class CollisionTestHelper : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[{gameObject.name}] ha colisionado con [{collision.gameObject.name}]");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[{gameObject.name}] ha activado trigger con [{other.gameObject.name}]");
    }
}
