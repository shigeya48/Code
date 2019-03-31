using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text playerHpUI;

    public Slider playerHpSlider;

    ThrowHook player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>();
    }

    // Update is called once per frame
    void Update()
    {
        float playerHp = player.PlayerHP();
        //playerHpUI.text = ("HP : " + playerHp + " /  100");

        playerHpSlider.value = playerHp;
    }
}
