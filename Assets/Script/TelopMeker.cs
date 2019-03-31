using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelopMeker : SingletonMonoBehaviour<TelopMeker> {

    [SerializeField, Header("最初のテキスト")]
   GameObject ClickText;
    [SerializeField, Header("コアのテキスト")]
    GameObject CoerText;
    [SerializeField, Header("最後のテキスト")]
    GameObject EndText;
    

   
    public Vector2 guiScreenSize = new Vector2(1280, 720);	// 基準とする解像度
    public Rect rect;
    enum State{
        Click,
        Core,
        Rast,
        End,
    }

   State state;
	// Use this for initialization
	void Start () {
        state = State.End;
	}
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case State.Click:
                ClickText.SetActive(true);
                CoerText.SetActive(false);
                EndText.SetActive(false);
                break;

            case State.Core:
                ClickText.SetActive(false);
                CoerText.SetActive(true);
                EndText.SetActive(false);
                break;

            case State.Rast:
                ClickText.SetActive(false);
                CoerText.SetActive(false);
                EndText.SetActive(true);
                break;

            case State.End:
                ClickText.SetActive(false);
                CoerText.SetActive(false);
                EndText.SetActive(false);
                break;
        }
      
       
    }
    public void Click()
    {
        state = State.Click;
    }
    public void Core()
    {
        state = State.Core;
    }
    public void Rast()
    {
        state = State.Rast;
    }
    public void End()
    {
        state = State.End;
    }
  
}
