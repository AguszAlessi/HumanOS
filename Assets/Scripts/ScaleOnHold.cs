using UnityEngine;

[RequireComponent(typeof(OVRGrabbable))]
public class ScaleOnGrabOVR : MonoBehaviour
{
    [SerializeField] private float holdTime = 3f;       // segundos de sostener para escalar
    [SerializeField] private float scaleFactor = 1.5f;  // 1.5 = 150% del tamaño original
    [SerializeField] private bool keepScaledAfterRelease = false; // ¿se queda grande?
    [SerializeField] private float returnSpeed = 6f;    // velocidad de volver al tamaño original

    private OVRGrabbable grabbable;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float holdTimer;
    private bool wasGrabbedLastFrame = false; // nuevo flag
    private bool shouldReturn = false;        // solo true al soltar

    void Awake()
    {
        grabbable = GetComponent<OVRGrabbable>();

        if (grabbable == null)
            Debug.LogError($"[ScaleOnGrabOVR] No se encontró OVRGrabbable en {gameObject.name}");

        initialScale = transform.localScale;
        targetScale = initialScale * scaleFactor;

        Debug.Log($"[ScaleOnGrabOVR] Inicializado en {gameObject.name}. Tamaño inicial: {initialScale}");
    }

    void Update()
    {
        if (grabbable.isGrabbed)
        {
            // Reset de flags
            shouldReturn = false;
            wasGrabbedLastFrame = true;

            // Contador de tiempo y escalado progresivo
            holdTimer = Mathf.Min(holdTimer + Time.deltaTime, holdTime);
            float t = holdTime <= 0f ? 1f : holdTimer / holdTime; // 0..1
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            Debug.Log($"[ScaleOnGrabOVR] {gameObject.name} agarrado. Timer: {holdTimer:F2}/{holdTime}");
        }
        else
        {
            // Se acaba de soltar
            if (wasGrabbedLastFrame)
            {
                Debug.Log($"[ScaleOnGrabOVR] {gameObject.name} soltado.");
                wasGrabbedLastFrame = false;
                holdTimer = 0f;

                if (!keepScaledAfterRelease)
                    shouldReturn = true;
            }

            // Solo vuelve al tamaño original si debería hacerlo
            if (shouldReturn)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, initialScale, Time.deltaTime * returnSpeed);

                if ((transform.localScale - initialScale).sqrMagnitude < 0.0001f)
                {
                    transform.localScale = initialScale;
                    shouldReturn = false;
                    Debug.Log($"[ScaleOnGrabOVR] {gameObject.name} volvió a tamaño original.");
                }
            }
        }
    }
}
