using UnityEngine;

public class ZeroGMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float moveSpeed = 3.0f;
    public Transform headTransform;
    private Rigidbody rb;
    private bool isColliding = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (headTransform == null && Camera.main != null)
        {
            headTransform = Camera.main.transform;
        }
        rb.useGravity = false;
        rb.drag = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isColliding) return; // Si está colisionando, no mover

        Vector3 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        Vector3 direction = Vector3.zero;
        if (headTransform != null)
        {
            Vector3 forward = headTransform.forward;
            Vector3 right = headTransform.right;
            direction = forward * input.y + right * input.x;
            direction.Normalize();
        }

        Vector3 desiredVelocity = direction * moveSpeed;
        rb.velocity = desiredVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
        Debug.Log("Colision en ZeroGMovement con: " + collision.gameObject.name);
    }

    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}
