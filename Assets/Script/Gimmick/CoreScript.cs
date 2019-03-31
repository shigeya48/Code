using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour
{
    float greenData;
    float blueData;

    int num = 0;

    Color color;

    public Material[] lightMaterial;

    bool infection = false;
    bool infectionStart = false;

    GameObject lightingObject;

    public GameObject completeImage;

    public GameObject percentCanvas;

    // Use this for initialization
    void Start()
    {
        percentCanvas.SetActive(false);

        lightingObject = transform.GetChild(1).gameObject;

        lightingObject.GetComponent<Renderer>().material = lightMaterial[0];

        color = new Color(0.0f, 0.6f, 1.0f);

        greenData = color.g / 2;
        blueData = color.b / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!infectionStart)
        {
            if (color.g >= 0.6)
            {
                color.g = 0.6f;
                color.b = 1.0f;
                greenData *= -1;
                blueData *= -1;
            }
            else if (color.g <= 0)
            {
                color.g = 0;
                color.b = 0;
                greenData *= -1;
                blueData *= -1;
            }

            color.g += greenData * Time.deltaTime;
            color.b += blueData * Time.deltaTime;
        }

        lightMaterial[num].EnableKeyword("_EMISSION");
        lightMaterial[num].SetColor("_EmissionColor", color);
    }

    public bool InfectionFlg()
    {
        return infection;
    }

    public void Infection()
    {
        GetComponent<CircleCollider2D>().enabled = false;

        infectionStart = true;

        num = 1;
        lightingObject.GetComponent<Renderer>().material = lightMaterial[1];

        color = new Color(1.0f, 0.0f, 0.0f);

        StartCoroutine(InfectionEffect());
    }

    IEnumerator InfectionEffect()
    {
        float speed = 0.5f;
        float direction = 1.0f;

        yield return new WaitForSeconds(0.8f);

        SoundManager.Instance.MCore_Play();
        EffectManager.Instance.MCoreLoadEffect((Vector2)transform.position + new Vector2(0, 6f));

        percentCanvas.SetActive(true);

        float timer = 0;

        while (timer < 2.5f)
        {
            timer += Time.deltaTime;

            if (color.r >= 1)
            {
                color.r = 1;
                direction *= -1;
            }
            else if (color.r <= 0)
            {
                color.r = 0;
                direction *= -1;
            }

            color.r += 1.0f * speed * Time.deltaTime * direction;
            if (speed < 14)
            {
                speed += 0.1f;
            }

            yield return null;
        }

        SoundManager.Instance.Complete_Play();
        Instantiate(completeImage, transform.position + new Vector3(0, 6, 0), Quaternion.identity);

        color = new Color(1.0f, 0.0f, 0.0f);

        yield return new WaitForSeconds(1.0f);

        percentCanvas.SetActive(false);

        num = 2;
        lightingObject.GetComponent<Renderer>().material = lightMaterial[2];
        infection = true;

        lightMaterial[1].EnableKeyword("_EMISSION");
        lightMaterial[1].SetColor("_EmissionColor", new Color(1.0f, 0.0f, 0.0f));

        yield return new WaitForSeconds(1.0f);

        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().simulated = true;
        GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Animator>().SetBool("Infection", false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().inputCancel = false;

    }
}
