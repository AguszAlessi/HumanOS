using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotation : MonoBehaviour
{
    public InputActionProperty turnInput; // joystick derecho
    public float turnSpeed = 60f; // para giro suave
    public float snapAngle = 45f; // para snap
    public float deadzone = 0.6f;
    public float debounceTime = 0.3f;
    public bool useSnap = true;

    private float lastSnapTime;

    void Update()
    {
        Vector2 turn = turnInput.action.ReadValue<Vector2>();

        if (useSnap)
        {
            if (Time.time - lastSnapTime > debounceTime)
            {
                if (turn.x > deadzone)
                {
                    transform.Rotate(Vector3.up * snapAngle);
                    lastSnapTime = Time.time;
                }
                else if (turn.x < -deadzone)
                {
                    transform.Rotate(Vector3.up * -snapAngle);
                    lastSnapTime = Time.time;
                }
            }
        }
        else
        {
            transform.Rotate(Vector3.up * turn.x * turnSpeed * Time.deltaTime);
        }
    }
}
