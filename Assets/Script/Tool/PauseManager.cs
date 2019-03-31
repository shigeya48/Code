using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    bool clickFlg = false;

    public void ToTitle()
    {
        if (!clickFlg)
        {
            clickFlg = true;

            Time.timeScale = 1;

            SoundManager.Instance.PauseButton_Play();

            LoadScenes.Instance.FadeOut("Title");
        }
    }
    public void BackGame()
    {
        if (!clickFlg)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().PauseBackGame();
        }
    }
}
