using UnityEngine;
using Oculus;
using Oculus.Platform;
using static OVRInput;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ClimbHand : MonoBehaviour
{
    [Header("Meta / Mano")]
    public Controller controller = Controller.LTouch;
    public ClimbProvider climbProvider;
    public Transform handAnchor;

    [Header("Detección")]
    public LayerMask climbableLayers = ~0;
    public bool requireClimbableComponent = true;

    [Tooltip("Umbral para INICIAR agarre (hand trigger).")]
    [Range(0.05f, 0.99f)] public float gripPressThreshold = 0.55f;
    [Tooltip("Umbral más bajo para SOLTAR (histeresis).")]
    [Range(0.01f, 0.99f)] public float gripReleaseThreshold = 0.40f;
    [Tooltip("Tiempo de gracia para considerar contacto aunque haya parpadeo de trigger exit.")]
    [Range(0.0f, 0.25f)] public float contactGraceTime = 0.08f;

    [Header("Feeling / Estabilidad")]
    public float climbGain = 1.0f;
    public float verticalGain = 1.0f;
    public float maxStep = 0.25f;
    public float deadzone = 0.004f;
    [Range(0f, 0.9f)] public float smoothFactor = 0.25f;

    [Header("Colisión de la mano")]
    public float handSphereRadius = 0.04f;
    public float gripSphereRadius = 0.055f;

    SphereCollider _sphere;
    Rigidbody _rb;
    bool _isGripping;
    bool _isTouchingClimbable;
    float _lastTouchTime;
    Vector3 _lastHandWorldPos;
    Vector3 _smoothedDelta;
    int _gripStartFrame = -1;

    void Reset()
    {
        var sphere = GetComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = 0.04f;

        var rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Awake()
    {
        _sphere = GetComponent<SphereCollider>();
        _sphere.isTrigger = true;
        _sphere.radius = handSphereRadius;

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

        if (!handAnchor) handAnchor = transform;
        if (!climbProvider) climbProvider = FindObjectOfType<ClimbProvider>();
        if (gripReleaseThreshold > gripPressThreshold)
            gripReleaseThreshold = gripPressThreshold - 0.01f;
    }

    void LateUpdate()
    {
        float grip = Get(Axis1D.PrimaryHandTrigger, controller);
        bool touchingNowOrRecently = _isTouchingClimbable || (Time.time - _lastTouchTime) <= contactGraceTime;

        if (!_isGripping)
        {
            if (grip >= gripPressThreshold && touchingNowOrRecently)
            {
                _isGripping = true;
                _gripStartFrame = Time.frameCount;
                _lastHandWorldPos = handAnchor.position;
                _smoothedDelta = Vector3.zero;
                _sphere.radius = gripSphereRadius;
                if (climbProvider) climbProvider.RequestBeginClimb();
            }
        }
        else
        {
            if (Time.frameCount == _gripStartFrame)
                return; // Evita movimiento en el primer frame del agarre

            Vector3 rawDelta = handAnchor.position - _lastHandWorldPos;
            if (rawDelta.sqrMagnitude < (deadzone * deadzone)) rawDelta = Vector3.zero;
            _smoothedDelta = Vector3.Lerp(_smoothedDelta, rawDelta, 1f - Mathf.Clamp01(smoothFactor));

            Vector3 delta = _smoothedDelta * climbGain;
            delta.y *= verticalGain;
            if (delta.sqrMagnitude > maxStep * maxStep)
                delta = delta.normalized * maxStep;

            if (delta != Vector3.zero && climbProvider)
                climbProvider.MoveRigBy(-delta);

            _lastHandWorldPos = handAnchor.position;

            bool wantsRelease = grip <= gripReleaseThreshold;
            bool lostContactBeyondGrace = !touchingNowOrRecently;

            if (wantsRelease || lostContactBeyondGrace)
            {
                _isGripping = false;
                _sphere.radius = handSphereRadius;
                if (climbProvider) climbProvider.RequestEndClimb();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsValidClimbable(other)) return;
        _isTouchingClimbable = true;
        _lastTouchTime = Time.time;
    }

    void OnTriggerStay(Collider other)
    {
        if (!IsValidClimbable(other)) return;
        _isTouchingClimbable = true;
        _lastTouchTime = Time.time;
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsValidClimbable(other)) return;
        _isTouchingClimbable = false;
    }

    bool IsValidClimbable(Collider col)
    {
        if (((1 << col.gameObject.layer) & climbableLayers) == 0) return false;
        if (!requireClimbableComponent) return true;
        return col.GetComponentInParent<Climbable>() != null;
    }

    void OnDrawGizmosSelected()
    {
        if (!_sphere) _sphere = GetComponent<SphereCollider>();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_sphere ? _sphere.center : Vector3.zero, _sphere ? _sphere.radius : handSphereRadius);
    }
}
