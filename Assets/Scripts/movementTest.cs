using UnityEngine;

// Jetpack vertical para CharacterController + FirstPersonLocomotor.
// Poner en el *mismo* GO que tiene el CharacterController.
// En FirstPersonLocomotor: Jump Force = 0, Gravity Factor = 0.
[RequireComponent(typeof(CharacterController))]
public class JetpackLocomotor : MonoBehaviour
{
    [Header("Jetpack")]
    public float jetpackAccel = 8f;        // aceleración al mantener
    public float maxJetpackSpeed = 4.5f;   // límite de ascenso
    public float gravity = 9.81f;          // caída cuando no se pulsa
    public float maxFallSpeed = 12f;       // límite de caída
    public OVRInput.Button jetButton = OVRInput.Button.One;

    CharacterController cc;
    float verticalSpeed;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Mantengo lógica/estado aquí para que isGrounded se evalúe a tiempo,
        // pero NO muevo al controller hasta LateUpdate.
        bool jetHeld = OVRInput.Get(jetButton, OVRInput.Controller.RTouch);

        // “pegado” al piso cuando estamos grounded y no ascendemos
        if (cc.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -1f;

        if (jetHeld)
        {
            verticalSpeed += jetpackAccel * Time.deltaTime;
        }
        else
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }

        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxFallSpeed, maxJetpackSpeed);
    }

    void LateUpdate()
    {
        // Mover DESPUÉS de que el Locomotor movió en XZ
        Vector3 moveY = new Vector3(0f, verticalSpeed, 0f) * Time.deltaTime;
        cc.Move(moveY);
    }
}
