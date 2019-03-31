using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrearScript : MonoBehaviour
{
    public Material CoreCircle;

    public GameObject CoreLinePre;

    public GameObject completeImage;

    public Image clearImage;

    public GameObject percentCanvas;

    Image[] InfectionCompleted = new Image[18];

    Color color;

    bool infection = false;

    float greenData;
    float blueData;

    int colorChangeSpeed = 2;

    // Use this for initialization
    void Start()
    {
        color = new Color(0.0f, 0.6f, 1.0f);

        clearImage.color = new Color(1, 1, 1, 0);

        for (int i = 0; i < clearImage.transform.childCount; i++)
        {
            InfectionCompleted[i] = clearImage.transform.GetChild(i).GetComponent<Image>();
            InfectionCompleted[i].color = new Color(1, 1, 1, 0);
        }

        greenData = color.g / colorChangeSpeed;
        blueData = color.b / colorChangeSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (!infection)
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

        CoreCircle.EnableKeyword("_EMISSION");
        CoreCircle.SetColor("_EmissionColor", color);
    }

    public void InfectionCore()
    {
        GetComponent<CircleCollider2D>().enabled = false;

        infection = true;

        color = new Color(1.0f, 0.0f, 0.0f);

        StartCoroutine(InfectionEffect());
    }

    IEnumerator InfectionEffect()
    {
        yield return new WaitForSeconds(1.0f);

        float speed = 1.0f;
        float direction = 1.0f;

        float timer = 0;

        EffectManager.Instance.CoreLoadEffect((Vector2)transform.position);
        percentCanvas.SetActive(true);
        SoundManager.Instance.Core_Play();

        while (timer < 5)
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
                speed += 0.05f;
            }

            yield return null;
        }
        
        color = new Color(1.0f, 0.0f, 0.0f);

        GameObject CoreLineEffect = Instantiate(CoreLinePre, transform.position, Quaternion.identity);

        float a = 0.7f;

        float aSpeed = 0.3f;

        CoreLineEffect.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, a);

        while (CoreLineEffect.GetComponent<SpriteRenderer>().color.a > 0)
        {
            a -= aSpeed * Time.deltaTime;

            aSpeed += 0.1f;

            CoreLineEffect.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, a);

            CoreLineEffect.transform.localScale += new Vector3(2f, 2f, 0) * Time.deltaTime;
            yield return null;
        }

        SoundManager.Instance.Complete_Play();
        Instantiate(completeImage, transform.position + new Vector3(0, 8, 0), Quaternion.identity);

        Destroy(CoreLineEffect);
        percentCanvas.SetActive(false);

        yield return new WaitForSeconds(2.0f);

        EffectManager.Instance.PlayerLostEffect(GameObject.FindGameObjectWithTag("Player").transform.position);

        GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        Color panelColor = new Color(0.7f, 0.7f, 0.7f, 0);

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraControler>().cameraStopFlg = true;

        while (panelColor.a < 1f)
        {
            panelColor.a += 1.0f * Time.deltaTime;

            if (panelColor.a  > 1f)
            {
                panelColor.a = 1f;
            }

            clearImage.color = panelColor;

            yield return null;
        }

        while (panelColor.a > 0.8f)
        {
            panelColor.a -= 1.0f * Time.deltaTime;

            if (panelColor.a < 0.8f)
            {
                panelColor.a = 0.8f;
            }

            clearImage.color = panelColor;

            yield return null;
        }


        yield return new WaitForSeconds(2.0f);

        for (int i = 0; i < clearImage.transform.childCount; i++)
        {
			SoundManager.Instance.ClearText_Play();

            InfectionCompleted[i].color = new Color(1, 1, 1, 1);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2f);
        yield return null;

        LoadScenes.Instance.FadeOut("Title");
		while(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume > 0)
		{
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume -= 0.2f * Time.deltaTime;

			if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume < 0)
			{
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().volume = 0;
			}

			yield return null;
		}
    }
}
