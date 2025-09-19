using UnityEngine;
using Oculus.Interaction.Locomotion;

public class JetpackInput : MonoBehaviour
{
    [SerializeField] private FirstPersonLocomotor locomotor;
    [SerializeField] private OVRInput.Button jetButton = OVRInput.Button.One; // A (mando derecho)

    void Reset()
    {
        if (!locomotor) locomotor = GetComponent<FirstPersonLocomotor>();
    }

    void Update()
    {
        if (locomotor && OVRInput.Get(jetButton))
            locomotor.Jump();
    }
}