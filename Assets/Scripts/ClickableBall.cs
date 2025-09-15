using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
public class ClickableBall : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Renderer de la pelota/cubo (malla visible).")]
    public Renderer targetRenderer;

    [Tooltip("Script de brillo por emisión mientras está pendiente.")]
    public GlowPulse glowPulse;

    [Tooltip("Material de 'pendiente' con Emission activo (URP).")]
    public Material ballGlowMat;

    [Tooltip("Material sólido para estado 'clickeada'.")]
    public Material ballMarkedMat;

    [Tooltip("Referencia al GameManager en escena (si se deja vacío se busca en Start).")]
    public GameManager gameManager;

    [Header("Opcional")]
    [Tooltip("Punto donde spawnea el globo (si es null, usa transform.position + upOffset).")]
    public Transform bubbleAnchor;
    [SerializeField] private float upOffset = 0.35f;

    XRSimpleInteractable _xri;
    bool _clicked;

    void Reset()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
        glowPulse = GetComponent<GlowPulse>();
        _xri = GetComponent<XRSimpleInteractable>();
    }

    void Awake()
    {
        if (!targetRenderer) targetRenderer = GetComponentInChildren<Renderer>();
        if (!_xri) _xri = GetComponent<XRSimpleInteractable>();
        if (!_xri) _xri = gameObject.AddComponent<XRSimpleInteractable>(); // Garantiza eventos
    }

    void Start()
    {
        if (!gameManager) gameManager = FindFirstObjectByType<GameManager>();

        // Asegura material inicial (glow).
        if (targetRenderer && ballGlowMat)
        {
            targetRenderer.sharedMaterial = ballGlowMat;
            // Habilita keyword por si acaso.
            targetRenderer.sharedMaterial.EnableKeyword("_EMISSION");
        }

        // Eventos de click compatibles con el rig del Building Block.
        _xri.selectEntered.AddListener(OnSelectEntered);
        _xri.activated.AddListener(OnActivated);
    }

    void OnDestroy()
    {
        if (_xri)
        {
            _xri.selectEntered.RemoveListener(OnSelectEntered);
            _xri.activated.RemoveListener(OnActivated);
        }
    }

    void OnSelectEntered(SelectEnterEventArgs _)
    {
        TryClick();
    }

    void OnActivated(ActivateEventArgs _)
    {
        TryClick();
    }

    void TryClick()
    {
        if (_clicked) return;
        _clicked = true;

        // Apaga el pulso de emisión y fija material marcado.
        if (glowPulse) glowPulse.enabled = false;
        if (targetRenderer && ballMarkedMat)
        {
            targetRenderer.sharedMaterial = ballMarkedMat;
            ballMarkedMat.DisableKeyword("_EMISSION"); // aspecto sólido marcado
        }

        // Deshabilita futuros eventos de interacción.
        if (_xri) _xri.enabled = false;

        // Deshabilita DistanceGrab/Grab del SDK Meta si están presentes.
        DisableIfExists("DistanceGrabInteractable");
        DisableIfExists("GrabInteractable");

        // Notifica al GameManager.
        if (gameManager) gameManager.ReportBallClicked(this);
    }

    void DisableIfExists(string typeName)
    {
        // Evita dependencias duras: no rompe si el componente no está importado.
        var comp = GetComponent(typeName);
        if (comp is Behaviour b) b.enabled = false;
    }

    /// <summary>Devuelve la posición de spawn del globo de texto.</summary>
    public Vector3 GetBubbleWorldPos()
    {
        if (bubbleAnchor) return bubbleAnchor.position;
        return transform.position + Vector3.up * upOffset;
    }
}
