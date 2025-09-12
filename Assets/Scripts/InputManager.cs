// InputManager.cs  (sin clase generada)
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMotor))]
public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;   // arrastrá aquí tu asset o el componente
    InputAction jumpAction;
    PlayerMotor motor;

    void Awake()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();  // si el GO ya tiene PlayerInput

        motor = GetComponent<PlayerMotor>();

        // Busca el Action Map "OnFoot" y dentro la Action "Jump"
        var map = playerInput.actions.FindActionMap("OnFoot", throwIfNotFound: true);
        jumpAction = map.FindAction("Jump", throwIfNotFound: true);

        jumpAction.performed += ctx => motor.Jump();
    }

    void OnEnable()
    {
        jumpAction?.Enable();
    }

    void OnDisable()
    {
        jumpAction?.Disable();
    }
}
