using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ClimbProvider : MonoBehaviour
{
    [Header("XR Rig")]
    public Transform rigRoot; // debería apuntar a TrackingSpace
    public CharacterController characterController; // puede ser null si no usás uno explícito
    public GameObject locomotionBlock; // opcional: el bloque de locomoción del camera rig

    [Header("Debug")]
    public bool debugLogs = true;

    int _handsGripping = 0;
    bool _isClimbing = false;

    public void RequestBeginClimb()
    {
        _handsGripping++;
        if (_handsGripping == 1)
        {
            SetClimbing(true);
        }

        if (debugLogs)
            Debug.Log($"[CLIMB PROVIDER] RequestBeginClimb → Hands now: {_handsGripping}");
    }

    public void RequestEndClimb()
    {
        _handsGripping--;
        _handsGripping = Mathf.Max(0, _handsGripping);

        if (_handsGripping == 0)
        {
            SetClimbing(false);
        }

        if (debugLogs)
            Debug.Log($"[CLIMB PROVIDER] RequestEndClimb → Hands now: {_handsGripping}");
    }

    void SetClimbing(bool climbing)
    {
        _isClimbing = climbing;

        if (characterController)
        {
            characterController.enabled = !climbing;
        }

        if (locomotionBlock)
        {
            locomotionBlock.SetActive(!climbing);
            Debug.Log($"[CLIMB PROVIDER] {(climbing ? "Disabling" : "Enabling")} locomotion: {locomotionBlock.name}");
        }

        Debug.Log($"[CLIMB PROVIDER] SetClimbing = {climbing}");
    }

    public void MoveRigBy(Vector3 movement)
    {
        if (rigRoot != null && movement != Vector3.zero)
        {
            rigRoot.position += movement;
            Debug.Log($"[CLIMB PROVIDER] Moving rig by {movement:F3}");
        }
    }

    public bool IsClimbing => _isClimbing;
}
