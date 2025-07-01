using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject loadingPanel;

    [Header("Slider y Texto de carga")]
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

    // Esta función debe estar vinculada al botón Btn_PlayCampaign
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

        yield return new WaitForSeconds(1f); // para que se vea

        AsyncOperation operacion = SceneManager.LoadSceneAsync(sceneName);
        operacion.allowSceneActivation = false;

        while (operacion.progress < 0.9f)
        {
            if (sliderCarga != null)
            {
                float progreso = Mathf.Clamp01(operacion.progress / 0.9f);
                sliderCarga.value = progreso;
            }
            yield return null;
        }

        if (sliderCarga != null)
            sliderCarga.value = 1f;

        yield return new WaitForSeconds(0.5f);
        operacion.allowSceneActivation = true;
    }
} 
