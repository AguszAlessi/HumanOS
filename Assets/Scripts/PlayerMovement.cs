using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float moveSpeed = 3.0f;
    public float jetpackForce = 10f;
    public float maxStamina = 5f;
    public float staminaRecoveryRate = 1f;
    public float staminaConsumptionRate = 1f;

    [Header("Referencias")]
    public Transform headTransform; // Debe ser el CenterEyeAnchor
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.2f;

    private Rigidbody rb;
    private float currentStamina;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (headTransform == null && Camera.main != null)
        {
            headTransform = Camera.main.transform;
        }

        rb.useGravity = true;
        rb.drag = 1f;
        rb.freezeRotation = true;

        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleMovement();
        HandleJetpack();
    }

    void HandleMovement()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        if (input.magnitude > 0.1f)
        {
            Vector3 forward = Vector3.ProjectOnPlane(headTransform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(headTransform.right, Vector3.up).normalized;
            Vector3 direction = (forward * input.y + right * input.x).normalized;

            Vector3 targetVelocity = direction * moveSpeed;
            Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    void HandleJetpack()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);

        Vector2 rightThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.RTouch);
        bool usingJetpack = rightThumbstick.y > 0.5f;

        if (usingJetpack && currentStamina > 0f)
        {
            rb.AddForce(Vector3.up * jetpackForce, ForceMode.Acceleration);
            currentStamina -= staminaConsumptionRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
        else if (isGrounded)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    void OnGUI()
    {
        // Simple barra de stamina (debug visual)
        float barWidth = 200f;
        float barHeight = 20f;
        float filled = currentStamina / maxStamina;
        GUI.Box(new Rect(10, 10, barWidth, barHeight), "");
        GUI.Box(new Rect(10, 10, barWidth * filled, barHeight), "Stamina");
    }
}
