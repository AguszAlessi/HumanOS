using UnityEngine;

// Este script reemplaza a OVRGrabbable y además mantiene el objeto fijo
// pero permite disparar otros scripts (ej: ScaleOnGrabOVR) al detectar el grab.
public class FixedOVRGrabbable : OVRGrabbable
{
    private Rigidbody rb;

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);

        if (grabbedRigidbody != null)
        {
            rb = grabbedRigidbody;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Debug.Log($"[FixedOVRGrabbable] {gameObject.name} agarrado.");
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        // ignoramos la velocidad para que no intente moverse al soltar
        base.GrabEnd(Vector3.zero, Vector3.zero);

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        Debug.Log($"[FixedOVRGrabbable] {gameObject.name} soltado.");
    }
}
