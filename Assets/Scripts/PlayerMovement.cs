using UnityEngine;

public class ZeroGMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float moveSpeed = 3.0f;
    public Transform headTransform; // Debe ser el CenterEyeAnchor

    private Rigidbody rb;
    private Vector3 initialHeadLocalPosition;

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

        if (headTransform != null)
            initialHeadLocalPosition = headTransform.localPosition;
    }

    void Update()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        Vector3 direction = Vector3.zero;
        if (headTransform != null && input.magnitude > 0.1f)
        {
            Vector3 forward = Vector3.ProjectOnPlane(headTransform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(headTransform.right, Vector3.up).normalized;
            direction = forward * input.y + right * input.x;
            direction.Normalize();

            rb.velocity = direction * moveSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    void LateUpdate()
    {
        if (headTransform != null)
        {
            Vector3 delta = headTransform.localPosition - initialHeadLocalPosition;
            delta.y = 0f; // Eliminar movimiento vertical indeseado
            transform.position += transform.rotation * delta;

            headTransform.localPosition = initialHeadLocalPosition;
        }
    }
}
