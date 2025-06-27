using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabSound : MonoBehaviour
{
    public AudioClip grabBallAudio;
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (grabBallAudio != null && audioSource != null)
        {
            audioSource.PlayOneShot(grabBallAudio);
        }
    }
}