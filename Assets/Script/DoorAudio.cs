using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
    public AudioClip UnLookSound;
    public AudioClip OpenSound;

    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void UnLook()
    {
        audioSource.PlayOneShot(UnLookSound);
    }

    public void DoorOpen()
    {
        audioSource.PlayOneShot(OpenSound);
    }
}
