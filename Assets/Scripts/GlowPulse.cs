using UnityEngine;

/// <summary>
/// Pulso de emisión URP mediante MaterialPropertyBlock (apto para Quest 3).
/// Asignar en el Inspector: targetRenderer, emissionColor, min/maxIntensity, pulseSpeed.
/// </summary>
[DisallowMultipleComponent]
public class GlowPulse : MonoBehaviour
{
    [Header("Renderer objetivo (malla de la pelota/cubo)")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Emisión (HDR)")]
    [ColorUsage(true, true)]
    [SerializeField] private Color emissionColor = Color.cyan;
    [SerializeField, Min(0f)] private float minIntensity = 0.6f;
    [SerializeField, Min(0f)] private float maxIntensity = 2.2f;
    [SerializeField, Min(0.01f)] private float pulseSpeed = 1.25f;

    MaterialPropertyBlock _mpb;
    static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");
    float _t;

    void Reset()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
    }

    void Awake()
    {
        if (!targetRenderer) targetRenderer = GetComponentInChildren<Renderer>();
        _mpb = new MaterialPropertyBlock();
        // Garantiza que la palabra clave de Emission esté activa.
        if (targetRenderer && targetRenderer.sharedMaterial)
            targetRenderer.sharedMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (!targetRenderer) return;

        _t += Time.deltaTime * pulseSpeed;
        float s = (Mathf.Sin(_t) + 1f) * 0.5f;                   // 0..1
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, s);
        Color emissive = emissionColor * intensity;

        targetRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(EmissionColorID, emissive);
        targetRenderer.SetPropertyBlock(_mpb);
    }

    /// <summary>Fuerza un valor fijo de emisión y deja de pulsat (opcional).</summary>
    public void SetEmissionOnce(Color color)
    {
        if (!targetRenderer) return;
        targetRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor(EmissionColorID, color);
        targetRenderer.SetPropertyBlock(_mpb);
    }
}
