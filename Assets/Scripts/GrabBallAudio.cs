using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSound : MonoBehaviour
{
    private OVRGrabbable grabbable;
    private AudioSource audioSource;

    public AudioClip grabSound;

    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (grabbable != null && grabbable.isGrabbed && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(grabSound);
        }
    }
}
