using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    private XRNode inputSource = XRNode.LeftHand;
    private Vector2 inputAxis;
    public float speed = 5f; // Velocidad del personaje
    public float jumpForce = 5f;    // Fuerza del salto

    private Rigidbody rb;
    private bool isGrounded;        // ¿Está en el suelo?

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
        {
            // Leer entrada del teclado
            float moveX = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
            float moveZ = Input.GetAxis("Vertical");   // W/S o flechas arriba/abajo

            // Crear vector de movimiento
            Vector3 movement = new Vector3(moveX, 0f, moveZ);

            // Mover el Rigidbody (sin interferir con la física)
            Vector3 newPos = rb.position + movement * Time.deltaTime;
            rb.MovePosition(newPos);

            // Saltar si está en el suelo y presionamos espacio
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Ya no está en el suelo al saltar
            }
    


            // Mover el objeto directamente
            transform.position += movement * speed * Time.deltaTime;
        }

    // Cuanddo el player detecta el piso (con el tag Ground) se vuelve a activar la funcion que detecta cuando toca el piso
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}







// InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
//if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
//{
  //  Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);
  
    // Usamos la dirección de la cámara para mover relativo a donde mira el jugador
  //  Transform cameraTransform = Camera.main.transform;
    //Vector3 forward = cameraTransform.forward;
    //forward.y = 0;
    //forward.Normalize();

    //Vector3 right = cameraTransform.right;
    //right.y = 0;
    //right.Normalize();

    //Vector3 move = forward * direction.z + right * direction.x;
    //transform.position += move * speed * Time.deltaTime;
//}