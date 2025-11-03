using UnityEngine;

[DisallowMultipleComponent]
public class Climbable : MonoBehaviour
{
    [Tooltip("Opcional: 0=sin fricción, 1=agarre muy 'pegajoso'. Útil si luego querés regular deslizamiento.")]
    [Range(0f, 1f)] public float gripFriction = 1f;
}
