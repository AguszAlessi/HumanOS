using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class HeartbeatInJuegoScene : MonoBehaviour
{
    private AudioSource audioSource;
    private float targetPitch;
    private float pitchLerpSpeed = 1.5f; // Velocidad del cambio de pitch

    [Range(0.1f, 2f)]
    public float playbackSpeed = 0.8f;

    public string tagAntivirus = "Antivirus";

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.pitch = playbackSpeed;
        targetPitch = playbackSpeed;
    }

    void Start()
    {
        CheckSceneAndPlay();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndPlay();
    }

    private void CheckSceneAndPlay()
    {
        if (SceneManager.GetActiveScene().name == "ArteryGame")
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagAntivirus))
        {
            targetPitch = 1.7f;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagAntivirus))
        {
            targetPitch = playbackSpeed;
        }
    }

    void Update()
    {
        if (audioSource != null)
        {
            audioSource.pitch = Mathf.Lerp(audioSource.pitch, targetPitch, Time.deltaTime * pitchLerpSpeed);
        }
    }
}
