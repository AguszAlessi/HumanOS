using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public XRNode inputSource = XRNode.LeftHand;
    public float speed = 3f;
    private Vector2 inputAxis;

    private Rigidbody rb;
    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            // Dirección completa basada en la cámara, incluyendo inclinación
            Vector3 moveDirection = (cameraTransform.forward * inputAxis.y + cameraTransform.right * inputAxis.x).normalized;

            // Movimiento real en 3D (con altura incluida)
            Vector3 movement = moveDirection * speed * Time.deltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}
