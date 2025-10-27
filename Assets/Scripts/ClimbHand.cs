using UnityEngine;

public class ClimbHand : MonoBehaviour
{
    [SerializeField] ClimbProvider climbProvider;
    [SerializeField] bool isLeftHand = true;

    private bool touchingClimbable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable"))
            touchingClimbable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
            touchingClimbable = false;
    }

    void Update()
    {
        var button = isLeftHand
            ? OVRInput.Button.PrimaryHandTrigger
            : OVRInput.Button.SecondaryHandTrigger;

        if (touchingClimbable && OVRInput.GetDown(button))
            climbProvider.BeginClimb(transform);

        if (OVRInput.GetUp(button))
            climbProvider.EndClimb(transform);
    }
}
