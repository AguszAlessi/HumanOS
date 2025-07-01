using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject loadingPanel;

  
    [SerializeField] private Slider sliderCarga;
    [SerializeField] private Text loadingText;

    private void Start()
    {
        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);
        loadingPanel.SetActive(false);

        if (loadingText != null)
            loadingText.text = "";
    }

    
    public void Play()
    {
        StartCoroutine(LoadSceneAsync("ArteryGame"));
    }

    // Esta función puede ser llamada por Btn_ExitMenu en la escena del juego
    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
{
    panelMainMenu?.SetActive(false);
    panelSettings?.SetActive(false);
    loadingPanel?.SetActive(true);

    if (loadingText != null)
        loadingText.text = "Cargando...";

    AsyncOperation operacion = SceneManager.LoadSceneAsync(sceneName);
    operacion.allowSceneActivation = false;

    float progresoVisual = 0.5f;

    while (!operacion.isDone)
    {

        float progresoTarget = Mathf.Clamp01(operacion.progress / 0.9f);

        // Animar suavemente la barra con Lerp
        progresoVisual = Mathf.MoveTowards(progresoVisual, progresoTarget, Time.deltaTime * 0.5f);

        if (sliderCarga != null)
            sliderCarga.value = progresoVisual;

        if (loadingText != null)
            loadingText.text = $"Cargando... {Mathf.RoundToInt(progresoVisual * 100)}%";
        if (progresoVisual >= 1f && operacion.progress >= 0.9f)
        {
            yield return new WaitForSeconds(0.2f); 
            operacion.allowSceneActivation = true;
        }

        yield return null;
    }
}

} 
