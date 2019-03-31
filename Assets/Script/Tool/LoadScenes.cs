using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScenes : SingletonMonoBehaviour<LoadScenes> {

    [SerializeField]
    private float fadeInTime;//ここ弄ればフェードの速さも変わる
    float F=0;
    float A = 10;
    float Alfa=5;
    private Image image;
    string scenes;
    bool Load;
    // Use this for initialization
    void Start ()
    {
        image = transform.Find("Panel").GetComponent<Image>();
        //　コルーチンで使用する待ち時間を計測
        fadeInTime = 1f * fadeInTime / 10f;
        image = transform.Find("Panel").GetComponent<Image>();
        StartCoroutine("FadeIn");
    }
	
	// Update is called once per frame
	void Update () {
        if (Load==true) {
            
            if (F >=Alfa)
            {
                SceneManager.LoadScene(scenes);
            }
        }
    }

    public void FadeOut(string sceneName)//ここ呼び出せばロードとフェードアウトが始まる
    {
        scenes = sceneName;
        Load = true;
        StartCoroutine(Out());
    }

    IEnumerator Out()
    {
        for (float i = 1f; i<=A; F += 0.1f)
        { 
        image.color = new Color(0f, 0f, 0f, F); 
        //　指定秒数待つ
        yield return new WaitForSeconds(fadeInTime);
          
        }
    }
    IEnumerator FadeIn()
    {
        //　Colorのアルファを0.1ずつ下げていく
        for (var i = 1f; i >= 0; i -= 0.1f)
        {
            image.color = new Color(0f, 0f, 0f, i);
            //　指定秒数待つ
            yield return new WaitForSeconds(fadeInTime);
        }
    }
}
