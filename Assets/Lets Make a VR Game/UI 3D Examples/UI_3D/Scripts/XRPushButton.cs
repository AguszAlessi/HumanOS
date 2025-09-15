using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]   // el collider será el “área de click/toque”
public class XRPushButton : MonoBehaviour
{
    [Header("Asigná la tapa del botón (la que baja)")]
    public Transform plunger;                    // p.ej. EmergencyStop (hijo)

    [Header("Movimiento")]
    public float downDistance = 0.02f;           // 2 cm hacia abajo
    public float travelTime  = 0.08f;            // bajada y subida
    public float holdTime    = 0.06f;            // queda apretado un instante
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1);

    [Header("Disparo")]
    public bool singleShot = true;               // ignora nuevos toques hasta terminar
    public UnityEvent onPressed;                 // al iniciar la bajada (opc.)
    public UnityEvent onPressedCompleted;        // al terminar la subida (tu acción)
    public UnityEvent onReleased;                // al iniciar la subida (opc.)

    Vector3 _startLocalPos;
    bool _isAnimating;

    void Awake()
    {
        if (!plunger)
            Debug.LogError("XRPushButton: asigná el 'plunger' (tapa del botón).", this);
        else
            _startLocalPos = plunger.localPosition;

        // por si el collider no está marcado como trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    // ───────────────────────────── Interacciones XR ─────────────────────────────
    // Si usás XR Simple Interactable: conectá SelectEntered -> Press()
    public void Press(SelectEnterEventArgs _) => Press();
    public void Press()
    {
        if (_isAnimating && singleShot) return;
        StartCoroutine(AnimatePress());
    }

    // Si preferís “toque físico” (poke con collider de la mano/control)
    void OnTriggerEnter(Collider other)
    {
        // exige rigidbody para evitar falsos positivos
        if (!other.attachedRigidbody) return;
        Press();
    }

    // ───────────────────────────── Animación ─────────────────────────────
    IEnumerator AnimatePress()
    {
        _isAnimating = true;
        onPressed?.Invoke();

        // Bajar
        yield return MoveLocalY(_startLocalPos,
                                _startLocalPos + Vector3.down * downDistance,
                                travelTime);

        // Apretado un instante
        yield return new WaitForSeconds(holdTime);

        onReleased?.Invoke();

        // Subir
        yield return MoveLocalY(plunger.localPosition, _startLocalPos, travelTime);

        // Acción final (solo cuando volvió a su lugar)
        onPressedCompleted?.Invoke();

        _isAnimating = false;
    }

    IEnumerator MoveLocalY(Vector3 from, Vector3 to, float t)
    {
        float e = 0f;
        while (e < t)
        {
            e += Time.deltaTime;
            float k = Mathf.Clamp01(e / t);
            float s = ease.Evaluate(k);
            plunger.localPosition = Vector3.LerpUnclamped(from, to, s);
            yield return null;
        }
        plunger.localPosition = to;
    }
}
