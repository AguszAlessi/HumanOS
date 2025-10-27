using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ClimbProvider : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] Transform rigRoot;              // El root del Camera Rig
    [SerializeField] GameObject locomotor;           // (Opcional) arrastrá el "Locomotor" del bloque OVRInteractionComprehensive

    private CharacterController controller;
    private readonly List<Transform> activeHands = new();
    private readonly Dictionary<Transform, Vector3> lastPos = new();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (rigRoot == null) rigRoot = transform;
    }

    void Update()
    {
        if (activeHands.Count == 0) return;

        Vector3 totalDelta = Vector3.zero;
        foreach (Transform hand in activeHands)
        {
            Vector3 current = hand.position;
            Vector3 delta = current - lastPos[hand];
            totalDelta += delta;
            lastPos[hand] = current;
        }

        if (totalDelta.sqrMagnitude > 0.000001f)
            controller.Move(-totalDelta / activeHands.Count);
    }

    public void BeginClimb(Transform hand)
    {
        if (!activeHands.Contains(hand))
        {
            activeHands.Add(hand);
            lastPos[hand] = hand.position;
        }

        // Desactiva locomoción mientras escalás
        if (locomotor != null) locomotor.SetActive(false);
    }

    public void EndClimb(Transform hand)
    {
        if (activeHands.Contains(hand))
        {
            activeHands.Remove(hand);
            lastPos.Remove(hand);
        }

        // Si no queda ninguna mano trepando, reactiva locomoción
        if (activeHands.Count == 0 && locomotor != null)
            locomotor.SetActive(true);
    }
}
