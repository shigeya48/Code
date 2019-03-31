using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseInput : MonoBehaviour
{
    GlitchFx glitch;

    void Start()
    {
        glitch = FindObjectOfType<GlitchFx>().GetComponent<GlitchFx>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            glitch.intensity = 0.6f;
        }
    }
}