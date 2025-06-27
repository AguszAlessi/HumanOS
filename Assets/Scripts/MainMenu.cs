using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject panelMainMenu;
    public GameObject panelSettings;
    public GameObject LoadingPanel;
    public Slider sliderCarga;

    void Start()
    {
        
        panelMainMenu.SetActive(true);
        panelSettings.SetActive(false);
        LoadingPanel.SetActive(false);
    }

    public void Play()
    {
        
        StartCoroutine(Loading("ArteryGame"));
    }


IEnumerator Loading(string SceneName)
{
    // Activamos el panel de carga
    LoadingPanel.SetActive(true);

    // Aseguramos que los demás paneles se oculten
    panelSettings.SetActive(false);
    panelMainMenu.SetActive(false);

    // Fondo negro visible (asegúrate que sea un Image negro en el panel)
    // Se puede poner por Inspector, no hace falta hacer nada en código

    // Comenzamos la carga asíncrona
    AsyncOperation operacion = SceneManager.LoadSceneAsync(SceneName);

    // Esperamos mientras se carga
    while (!operacion.isDone)
    {
        float progreso = Mathf.Clamp01(operacion.progress / 0.9f);
        sliderCarga.value = progreso;
        yield return null;
    }
}

public void Settings()
{
    Debug.Log("⚙️ Se presionó el botón de Settings");

    panelMainMenu.SetActive(false);
    panelSettings.SetActive(true);

    Debug.Log("MAIN activo? " + panelMainMenu.activeSelf);
    Debug.Log("SETTINGS activo? " + panelSettings.activeSelf);
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
