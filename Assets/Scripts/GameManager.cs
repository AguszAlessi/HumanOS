using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    [Header("Prefab del globo")]
    [Tooltip("Prefab world-space con TextMeshProUGUI. Sin scripts extra.")]
    public GameObject floatingBubblePrefab;

    [Header("FX Globo")]
    [Tooltip("Segundos visibles antes del fade.")]
    public float bubbleLife = 1.2f;
    [Tooltip("Duración del fade-out.")]
    public float bubbleFade = 0.5f;
    [Tooltip("Altura extra sobre el anchor de la pelota.")]
    public float bubbleExtraUp = 0.05f;

    List<ClickableBall> _balls = new List<ClickableBall>();
    int _remaining;
    Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        _balls.AddRange(FindObjectsByType<ClickableBall>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        foreach (var b in _balls)
            if (b && !b.gameManager) b.gameManager = this;

        _remaining = _balls.Count;
    }

    public void ReportBallClicked(ClickableBall ball)
    {
        _remaining = Mathf.Max(0, _remaining - 1);

        if (floatingBubblePrefab && ball)
        {
            Vector3 pos = ball.GetBubbleWorldPos() + Vector3.up * bubbleExtraUp;
            var go = Instantiate(floatingBubblePrefab, pos, Quaternion.identity);

            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp)
            {
                string msg = (_remaining == 1) ? "Queda 1 pelota" : $"Quedan {_remaining} pelotas";
                tmp.text = msg; // siempre muestra cuántas faltan (incluye 0)
            }

            if (_cam)
                go.transform.forward = (_cam.transform.position - go.transform.position).normalized;

            StartCoroutine(FadeAndDestroy(go));
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
