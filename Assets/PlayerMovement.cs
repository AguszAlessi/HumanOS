using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.0f;

    private XRNode inputSource = XRNode.LeftHand;
    private Vector2 inputAxis;

    private void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);

            // Usamos la dirección de la cámara para mover relativo a donde mira el jugador
            Transform cameraTransform = Camera.main.transform;
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 move = forward * direction.z + right * direction.x;
            transform.position += move * speed * Time.deltaTime;
        }
    }
}