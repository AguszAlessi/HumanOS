using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    public AudioClip sonidoClick;
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void ReproducirSonido()
    {
        if (sonidoClick != null)
            audioSource.PlayOneShot(sonidoClick);
    }
}
