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
        if (!_xri) _xri = gameObject.AddComponent<XRSimpleInteractable>(); // garantiza eventos

        // Logs de diagnóstico de componentes base
        var hasCol = GetComponent<Collider>() != null || GetComponentInChildren<Collider>() != null;
        var rb = GetComponent<Rigidbody>();
        Debug.Log($"[PelotaInteractuable] Awake → XRSimple:{(_xri!=null)}," +
                  $" Collider:{hasCol}, RB:{(rb!=null)}", this);
    }

    void Start()
    {
        if (!gameManager) gameManager = FindFirstObjectByType<GameManager>();

        // Material inicial (glow)
        if (targetRenderer && ballGlowMat)
        {
            targetRenderer.sharedMaterial = ballGlowMat;
            targetRenderer.sharedMaterial.EnableKeyword("_EMISSION");
        }

        // Suscripción de eventos
        _xri.selectEntered.AddListener(OnSelectEntered);
        _xri.activated.AddListener(OnActivated);
        _xri.hoverEntered.AddListener(OnHoverEntered);
        _xri.hoverExited.AddListener(OnHoverExited);

        // Logs de configuración del XRI
        var cols = _xri.colliders != null ? _xri.colliders.Count : 0;
        var managerName = _xri.interactionManager ? _xri.interactionManager.name : "NULL";
        Debug.Log($"[PelotaInteractuable] Start → Colliders:{cols}, " +
                  $"InteractionManager:{managerName}, Layer:{LayerMask.LayerToName(gameObject.layer)}", this);

        if (cols == 0)
            Debug.LogWarning($"[PelotaInteractuable] No hay colliders asignados en XRSimpleInteractable. " +
                             $"Agrega el SphereCollider a la lista Colliders.", this);
    }

    void OnDestroy()
    {
        if (_xri)
        {
            _xri.selectEntered.RemoveListener(OnSelectEntered);
            _xri.activated.RemoveListener(OnActivated);
            _xri.hoverEntered.RemoveListener(OnHoverEntered);
            _xri.hoverExited.RemoveListener(OnHoverExited);
        }
    }

    // ===== Eventos =====
    void OnHoverEntered(HoverEnterEventArgs _)
    {
        Debug.Log($"[PelotaInteractuable] Hover ENTER ({name})", this);
    }
    void OnHoverExited(HoverExitEventArgs _)
    {
        Debug.Log($"[PelotaInteractuable] Hover EXIT ({name})", this);
    }

    void OnSelectEntered(SelectEnterEventArgs _)
    {
        Debug.Log($"[PelotaInteractuable] SelectEntered ({name})", this);
        TryClick();
    }

    void OnActivated(ActivateEventArgs _)
    {
        Debug.Log($"[PelotaInteractuable] Activated ({name})", this);
        TryClick();
    }

    void TryClick()
    {
        if (_clicked)
        {
            Debug.Log($"[PelotaInteractuable] Ignorado: ya estaba clickeada ({name})", this);
            return;
        }
        _clicked = true;

        // Apaga pulso y fija material marcado
        if (glowPulse) glowPulse.enabled = false;
        if (targetRenderer && ballMarkedMat)
        {
            targetRenderer.sharedMaterial = ballMarkedMat;
            ballMarkedMat.DisableKeyword("_EMISSION");
        }
        Debug.Log($"[PelotaInteractuable] Cambié material a 'marcada' ({name})", this);

        // Deshabilita futuros eventos de interacción
        if (_xri) _xri.enabled = false;

        // Deshabilita DistanceGrab/Grab del SDK Meta si están
        DisableIfExists("DistanceGrabInteractable");
        DisableIfExists("GrabInteractable");

        // Notifica al GameManager
        if (gameManager)
        {
            Debug.Log($"[PelotaInteractuable] Reporto click al GameManager", this);
            gameManager.ReportBallClicked(this);
        }
        else
        {
            Debug.LogError("[PelotaInteractuable] GameManager es NULL. " +
                           "Asegurate de tener uno en escena.", this);
        }
    }

    void DisableIfExists(string typeName)
    {
        var comp = GetComponent(typeName);
        if (comp is Behaviour b)
        {
            b.enabled = false;
            Debug.Log($"[PelotaInteractuable] Deshabilité componente '{typeName}'", this);
        }
    }

    /// <summary>Posición para el texto flotante.</summary>
    public Vector3 GetBubbleWorldPos()
    {
        if (bubbleAnchor) return bubbleAnchor.position;
        return transform.position + Vector3.up * upOffset;
    }
}
