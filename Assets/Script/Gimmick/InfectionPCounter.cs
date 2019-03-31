using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionPCounter : MonoBehaviour
{
    public Text infectionPercentText;

    float percent = 0;

    public float speed = 40f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (percent < 100)
        {
            percent += speed * Time.deltaTime;

            if (percent > 100)
            {
                percent = 100;
            }
        }

        infectionPercentText.text = (percent.ToString("N0") + "%");
    }


}
