using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour {
    public AudioClip Title_Camera_Sound;
    public AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Camera_Sound_()
    {
        audioSource.PlayOneShot(Title_Camera_Sound);
    }
}
