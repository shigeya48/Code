using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    public AudioClip Player_Eat;
    public AudioClip Player_Tentacle;
    public AudioClip Player_Damage;
    public AudioClip Player_WhileDamage;
    public AudioClip Player_WhiteNoise;
    public AudioClip Gimmick_MCore;
    public AudioClip Gimmick_CompleteText;
    public AudioClip Gimmick_Core;
    public AudioClip Gimmick_ShutDown;
    public AudioClip Pause_Button;
	public AudioClip Clear_text;

    AudioSource[] audios;

    void Start()
    {
        audios = GetComponents<AudioSource>();
    }
    
    public void Eat_Play()
    {
        audios[0].PlayOneShot(Player_Eat);
    }

    public void Tentacle_Play()
    {
        audios[1].PlayOneShot(Player_Tentacle);

    }

    public void Damage_Play()
    {
        audios[0].PlayOneShot(Player_Damage);
    }

    public void WhileDamage_Play()
    {
        audios[2].clip = Player_WhileDamage;
        if (!audios[2].isPlaying)
        {
            audios[2].Play();
        }
    }

    public void WhiteNoise_Play()
    {
        audios[0].PlayOneShot(Player_WhiteNoise);
    }

    public void MCore_Play()
    {
        audios[0].PlayOneShot(Gimmick_MCore);
    }
 
    public void Complete_Play()
    {
        audios[0].PlayOneShot(Gimmick_CompleteText);
    }

    public void Core_Play()
    {
        audios[0].PlayOneShot(Gimmick_Core);
    }

    public void ShutDown_Play()
    {
        audios[0].PlayOneShot(Gimmick_ShutDown);
    }

    public void PauseButton_Play()
    {
        audios[0].PlayOneShot(Pause_Button);
    }

	public void ClearText_Play()
	{
		audios [0].PlayOneShot (Clear_text);
	}
}
