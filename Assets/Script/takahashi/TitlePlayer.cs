using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayer : MonoBehaviour {
    public AudioClip[] Title_Player_Sound;
    public AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }   
    void Sound_0()
    {
        audioSource.PlayOneShot(Title_Player_Sound[0]);
    }
    void Sound_1()
    {
        audioSource.PlayOneShot(Title_Player_Sound[1]);
    }
    void Sound_2()
    {
        audioSource.PlayOneShot(Title_Player_Sound[2]);
    }
    void Sound_3()
    {
        audioSource.PlayOneShot(Title_Player_Sound[3]);
    }
    void Sound_4()
    {
        audioSource.PlayOneShot(Title_Player_Sound[4]);
    }
    void Sound_5()
    {
        audioSource.PlayOneShot(Title_Player_Sound[5]);
    }
    void Sound_6()
    {
        audioSource.PlayOneShot(Title_Player_Sound[6]);
    }
    void Sound_7()
    {
        audioSource.PlayOneShot(Title_Player_Sound[7]);
    }
}
