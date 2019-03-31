using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleLode : MonoBehaviour {

    //非同期動作で使用するAsyncOperation
    private AsyncOperation async;
    //シーンロード中に表示するUI画面
    [SerializeField]
    private Image loadUI;
    [SerializeField]
    private GameObject startButton;//ロード開始ボタン
    [SerializeField, Header("ゲームシーンネーム")]
    string GamesScece;
    [SerializeField]
    private GameObject Fade;
    bool Load=false;
    float Alfa=0;
    public GameObject button;
    public GameObject Loading;
    [SerializeField]
    private float fadeInTime;//ここ弄ればフェードの速さも変わる

    public AudioClip button_Sound;

	public GameObject clickButtonText;

	bool clickFlg = false;

    AudioSource source;
    public void NextScene()
    {
		if (!clickFlg)
		{
			clickFlg = true;
			
			source.PlayOneShot(button_Sound);

			clickButtonText.SetActive (false);

			//　ロード画面UIをアクティブにする
			Load=true;
			//　コルーチンを開始
			StartCoroutine("fade");
		}

    }


    void Start()
    {
        loadUI.color = new Color(0f, 0f, 0f, 0f);
        Fade.SetActive(false);
        //　コルーチンで使用する待ち時間を計測
        fadeInTime = 1f * fadeInTime / 10f;
        Loading.SetActive(false);

        source = GetComponent<AudioSource>();
       
    }
    void Update()
    {

    }
  

  IEnumerator fade()
    {

        if (Load)
        {
            startButton.SetActive(false);
            Fade.SetActive(true);
            Loading.SetActive(true);
            for (float i = 1f; i >=Alfa; Alfa += 0.01f)
            {
                loadUI.color = new Color(0f, 0f, 0f, Alfa);
                yield return new WaitForSeconds(fadeInTime);
            }
        }
        if (Alfa >= 1f)
        {
            SceneManager.LoadSceneAsync(GamesScece);
        }
    }
}
