// PlayerMotor.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    CharacterController cc;
    Vector3 velocity;

    public float gravity = -9.81f;
    public float jumpHeight = 1.8f;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool grounded = cc.isGrounded;

        if (grounded && velocity.y < 0f)
            velocity.y = -2f;                      // pegado al piso estable

        velocity.y += gravity * Time.deltaTime;    // gravedad acumulada
        cc.Move(velocity * Time.deltaTime);        // solo eje Y por ahora
    }

    public void Jump()
    {
        if (cc.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
