using UnityEngine;


public class ZeroGMovement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    public float moveSpeed = 3.0f;          // Velocidad en metros/segundo
    public Transform headTransform;         // Referencia a la cámara (HMD) del jugador
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (headTransform == null && Camera.main != null)
        {
            headTransform = Camera.main.transform;
        }
        // Asegurarse de que la gravedad esté desactivada
        rb.useGravity = false;
        rb.drag = 0f;  // Puedes ajustar o quitar esta línea según la resistencia deseada
        // Bloquear rotaciones para evitar vuelcos indeseados
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Leer el input del joystick (eje 2D del thumbstick izquierdo)
        Vector3 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // Calcular la dirección de movimiento basada en la orientación del HMD
        Vector3 direction = Vector3.zero;
        if (headTransform != null)
        {
            Vector3 forward = headTransform.forward;
            Vector3 right = headTransform.right;
            // Usar la dirección de mirada (forward) para el eje vertical del stick,
            // y la derecha de la mirada (right) para el eje horizontal.
            direction = forward * input.y + right * input.x;
            direction.Normalize(); // Opcional: normalizar para impedir velocidades diagonales más altas
        }

        // Calcular la velocidad deseada
        Vector3 desiredVelocity = direction * moveSpeed;
        
        // Opcional: Si no hay input, opcionalmente frenar (por ejemplo, podría interpolar velocidad a cero)
        // En este caso, si no hay input el desiredVelocity será un vector cero (porque direction será zero).
        
        // Asignar la velocidad al Rigidbody (movimiento basado en física)
        rb.velocity = desiredVelocity;
    }
}