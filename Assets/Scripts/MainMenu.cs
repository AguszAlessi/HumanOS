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

    [Header("Opcional: Slider de carga")]
    [SerializeField] private Slider sliderCarga;

    private void Start()
    {
        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);
        loadingPanel.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(LoadSceneAsync("ArteryGame"));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        panelMainMenu.SetActive(false);
        panelSettings.SetActive(false);
        loadingPanel.SetActive(true);

        AsyncOperation operacion = SceneManager.LoadSceneAsync(sceneName);

        while (!operacion.isDone)
        {
            if (sliderCarga != null)
            {
                float progreso = Mathf.Clamp01(operacion.progress / 0.9f);
                sliderCarga.value = progreso;
            }
            yield return null;
        }
    }

    // Resto de tus métodos para Settings, ReturnMenu y Quit...
    public void Settings()
    {
        panelMainMenu.SetActive(false);
        panelSettings.SetActive(true);
    }

    public void ReturnMenu()
    {
        panelSettings.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}