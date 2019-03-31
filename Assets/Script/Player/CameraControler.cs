using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraControler : MonoBehaviour
{

    PostProcessingBehaviour behaviour;

    GrainModel.Settings grainSetting;
    ChromaticAberrationModel.Settings chromaticSetting;

    GlitchFx glitch;

    Vector2 mousePos;
    Vector2 playerPos;

    bool shakeFlg = false;

    bool damageFlg = false;

    public bool cameraStopFlg = false;

    float speed = 2.0f;

    // Use this for initialization
    void Start()
    {
        behaviour = GetComponent<PostProcessingBehaviour>();

        grainSetting.size = 3;
        grainSetting.intensity = 0;
        behaviour.profile.grain.settings = grainSetting;

        chromaticSetting.intensity = 0;
        behaviour.profile.chromaticAberration.settings = chromaticSetting;


        glitch = GetComponent<GlitchFx>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraStopFlg)
        {
            return;
        }

        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 CameraPos = Vector2.Lerp(playerPos, mousePos, 0.05f);

        if (!shakeFlg)
        {
            transform.position = CameraPos + new Vector3(0, 0, -10);
        }
    }

    public void Shake()
    {
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        shakeFlg = true;

        transform.position += new Vector3(0, -4, 0);

        yield return new WaitForSeconds(0.02f);

        transform.position += new Vector3(0, 8, 0);
        yield return null;

        shakeFlg = false;
    }

    IEnumerator DamageShake()
    {
        Vector3 pos = transform.position;

        for (int i = 0; i < 4; i++)
        {
            transform.position += new Vector3(1, 0, 0);

            yield return new WaitForSeconds(0.03f);

            transform.position = pos;

            transform.position += new Vector3(-1, 0, 0);

            yield return new WaitForSeconds(0.03f);
        }
    }

    public void DamageCamera()
    {
        if (!damageFlg)
        {
            StartCoroutine(DamageNoise());
        }
    }

    IEnumerator DamageNoise()
    {
        damageFlg = true;

        StartCoroutine(DamageShake());

        while (true)
        {
            grainSetting.intensity += speed * Time.deltaTime;
            chromaticSetting.intensity += speed * Time.deltaTime;

            behaviour.profile.grain.settings = grainSetting;
            behaviour.profile.chromaticAberration.settings = chromaticSetting;


            if (grainSetting.intensity >= 1.0f)
            {
                grainSetting.intensity = 1;
                speed *= -1;
            }
            if (grainSetting.intensity <= 0)
            {
                speed *= -1;
                grainSetting.intensity = 0;
                chromaticSetting.intensity = 0;
                behaviour.profile.grain.settings = grainSetting;
                behaviour.profile.chromaticAberration.settings = chromaticSetting;
                break;
            }
            yield return null;
        }

        damageFlg = false;
    }

    public void DeathCamera()
    {
        StartCoroutine(IsDeath());
    }

    IEnumerator IsDeath()
    {

        while (glitch.intensity < 1)
        {
            glitch.intensity += 0.8f * Time.deltaTime;
            if (glitch.intensity >= 1)
            {
                glitch.intensity = 1;
            }
            yield return null;
        }

        yield return null;
    }

    public void RestartCamera()
    {
        StartCoroutine(IsRestart());
    }

    IEnumerator IsRestart()
    {

        while (glitch.intensity > 0)
        {
            glitch.intensity += -0.8f * Time.deltaTime;
            if (glitch.intensity < 0)
            {
                glitch.intensity = 0;
            }
            yield return null;
        }

        yield return null;
    }
}
