using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerScript : MonoBehaviour
{
    public LayerMask layer;

    LineRenderer lr;

    public AudioClip razerSound;

    AudioSource source;

    float startOffset = 10.0f;
    float endOffset = 25.0f;

    float timer = 0;
    float timeInterval = 4;

    Vector3 startPos;
    Vector3 endPos;

    Vector3 preStartPos;
    Vector3 preEndPos;

    int directionNum = 0;

    int preDirectionNum = 0;

    [SerializeField]
    int directionCounter = 3;

    bool preparationEffectDone = false;
    bool prePlay = false;

    bool effectDone = true;

	bool damageFlg = false;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        source = GetComponent<AudioSource>();

        lr.positionCount = 2;

        preDirectionNum = directionNum + 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeInterval / 2)
        {
            preparationEffectDone = true;
        }

        if (timer > timeInterval)
        {
            timer = 0;
            
            directionNum++;

            if (directionNum > directionCounter)
            {
                directionNum = 0;
            }
            source.PlayOneShot(razerSound);

            effectDone = true;
            prePlay = false;
        }

        switch (preDirectionNum)
        {
            case 0:
                preStartPos = transform.position + new Vector3(startOffset, 0, 0);
                preEndPos = transform.position + new Vector3(endOffset, 0, 0);
                break;
            case 1:
                preStartPos = transform.position + new Vector3(0, -startOffset, 0);
                preEndPos = transform.position + new Vector3(0, -endOffset, 0);
                break;
            case 2:
                preStartPos = transform.position + new Vector3(-startOffset, 0, 0);
                preEndPos = transform.position + new Vector3(-endOffset, 0, 0);
                break;
        }

        switch (directionNum)
        {
            case 0:
                startPos = transform.position + new Vector3(startOffset, 0, 0);
                endPos = transform.position + new Vector3(endOffset, 0, 0);
                break;
            case 1:
                startPos = transform.position + new Vector3(0, -startOffset, 0);
                endPos = transform.position + new Vector3(0, -endOffset, 0);
                break;
            case 2:
                startPos = transform.position + new Vector3(-startOffset, 0, 0);
                endPos = transform.position + new Vector3(-endOffset, 0, 0);
                break;
        }

        if (effectDone)
        {
            Invoke("RazerPreparation", 2);
        }
        Razer(directionNum);
    }

    void RazerPreparation()
    {
        if (preparationEffectDone)
        {
            Vector2 effectPos = Vector2.Lerp((Vector2)preStartPos, (Vector2)preEndPos, 0.5f);
            Quaternion angle = Quaternion.Euler(0, 0, 0);

            if (preDirectionNum % 2 != 0)
            {
                angle = Quaternion.Euler(0, 0, 90.0f);
            }

            if (preparationEffectDone && !prePlay)
            {
                preDirectionNum++;

                if (preDirectionNum > directionCounter)
                {
                    preDirectionNum = 0;
                }

                prePlay = true;
                preparationEffectDone = false;
                EffectManager.Instance.RazerPreEffect(effectPos, angle);
            }
        }
    }

    void Razer(int direction)
    {
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);

        RaycastHit2D hit = Physics2D.Linecast((Vector2)startPos, (Vector2)endPos, layer);

        if (hit)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
				StartCoroutine (DamageEffectManager (hit.collider));
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().PlayerDamageWhile(10.0f);
            }
            else if (hit.collider.gameObject.tag == "Tentacle")
            {
				StartCoroutine (DamageEffectManager (hit.collider));
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().PlayerDamageWhile(5.0f);
            }
        }

        Vector2 effectPos = Vector2.Lerp((Vector2)startPos, (Vector2)endPos, 0.5f);
        Quaternion angle = Quaternion.Euler(0, 0, 0);

        if (direction % 2 != 0)
        {
            angle = Quaternion.Euler(0, 0, 90.0f);
        }

        if (effectDone)
        {
            effectDone = false;
            EffectManager.Instance.RazerEffect(effectPos, angle);
        }
    }

	IEnumerator DamageEffectManager(Collider2D col)
	{
		if (!damageFlg)
		{
			damageFlg = true;
			EffectManager.Instance.DamageEffect(col.gameObject.transform.position);

			yield return new WaitForSeconds(0.1f);

			damageFlg = false;
		}

		yield return null;
	}
}
