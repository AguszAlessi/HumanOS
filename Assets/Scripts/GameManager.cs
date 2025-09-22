using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [Header("Prefab del globo (World Space, con TextMeshProUGUI)")]
    public GameObject floatingBubblePrefab;

    [Header("FX Globo")]
    public float bubbleLife = 1.2f;
    public float bubbleFade = 0.5f;
    public float bubbleExtraUp = 0.05f;

    readonly List<ClickableBall> _balls = new List<ClickableBall>();
    int _remaining;
    int _total;
    Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        _balls.Clear();
        _balls.AddRange(FindObjectsByType<ClickableBall>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        foreach (var b in _balls)
            if (b && !b.gameManager) b.gameManager = this;

        _total = _balls.Count;
        _remaining = _total;

        Debug.Log($"[GameManager] Detectadas {_total} pelotas.");
        if (!floatingBubblePrefab)
            Debug.LogWarning("[GameManager] 'floatingBubblePrefab' no asignado. No se verá el 1/3 sobre la pelota.");
    }

    public void ReportBallClicked(ClickableBall ball)
    {
        if (_remaining <= 0)
        {
            Debug.LogWarning("[GameManager] Ya no quedaban pelotas por contar.");
            return;
        }

        if (ball && _balls.Contains(ball))
        {
            _balls.Remove(ball);
            _remaining = Mathf.Max(0, _balls.Count);
        }
        int clicked = _total - _remaining; // 1..N

        Debug.Log($"[GameManager] Click registrada. Progreso: {clicked}/{_total}. Quedan: {_remaining}");

        // Spawnea texto 1/3 encima de la pelota
        if (floatingBubblePrefab && ball)
        {
            Vector3 pos = ball.GetBubbleWorldPos() + Vector3.up * bubbleExtraUp;
            var go = Instantiate(floatingBubblePrefab, pos, Quaternion.identity);

            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp) tmp.text = $"{clicked}/{_total}";
            else Debug.LogWarning("[GameManager] El prefab no tiene TextMeshProUGUI.");

            if (_cam)
                go.transform.forward = (_cam.transform.position - go.transform.position).normalized;

            StartCoroutine(FadeAndDestroy(go));
        }

        if (_remaining == 0)
        {
            Debug.Log("[GameManager] ¡Nivel completado! Todas las pelotas fueron clickeadas.");
            // TODO: acá podrías llamar a tu siguiente lógica de nivel/puerta/etc.
        }
    }

    IEnumerator FadeAndDestroy(GameObject go)
    {
        if (!go) yield break;

        var cg = go.GetComponentInChildren<CanvasGroup>();
        if (!cg) cg = go.AddComponent<CanvasGroup>();
        cg.alpha = 1f;

        yield return new WaitForSeconds(bubbleLife);

        float t = 0f;
        while (t < bubbleFade)
        {
            t += Time.deltaTime;
            float k = 1f - Mathf.Clamp01(t / bubbleFade);
            cg.alpha = k;
            go.transform.position += Vector3.up * (0.06f * Time.deltaTime);
            yield return null;
        }

        Destroy(go);
    }
}
