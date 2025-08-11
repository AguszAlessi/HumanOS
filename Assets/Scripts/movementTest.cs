using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 5f;          // impulso inicial
    public float holdForce = 5f;          // fuerza extra mientras se mantiene el botón
    public float maxHoldTime = 0.5f;      // límite de tiempo de "sostener" (seguridad)
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private bool isGrounded;
    private float holdTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = Vector3.down * 0.9f;
            groundCheck = gc.transform;
        }
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Inicio del salto con botón A (RTouch)
        if (isGrounded && OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            Jump();
            holdTimer = 0f; // resetea el temporizador del "sostenido"
        }

        // Mientras se mantenga el botón, seguir aplicando fuerza hacia arriba
        if (!isGrounded && OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            if (holdTimer < maxHoldTime)
            {
                rb.AddForce(Vector3.up * holdForce, ForceMode.Acceleration);
                holdTimer += Time.deltaTime;
            }
        }

        // Al tocar piso, reiniciamos el temporizador
        if (isGrounded) holdTimer = 0f;
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
