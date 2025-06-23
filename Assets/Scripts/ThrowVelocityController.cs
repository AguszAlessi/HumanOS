using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class ThrowVelocityController : MonoBehaviour
{
    [Tooltip("Multiplier applied to linear velocity when the object is released.")]
    public float velocityMultiplier = 1f;
    [Tooltip("Multiplier applied to angular velocity when the object is released.")]
    public float angularVelocityMultiplier = 1f;

    XRGrabInteractable grabInteractable;
    new Rigidbody rigidbody;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rigidbody = GetComponent<Rigidbody>();
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        rigidbody.velocity *= velocityMultiplier;
        rigidbody.angularVelocity *= angularVelocityMultiplier;
    }
}