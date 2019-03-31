using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer2Script : MonoBehaviour
{
    public LayerMask layer;

    LineRenderer lr;

    Vector2 endPos;
    Vector2 effectPos;

    float endPosOffset = -24;

    bool damageFlg = false;

    bool razerFlg = false;

    public AudioClip razerSound;
    AudioSource sound;

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        sound = GetComponent<AudioSource>();

        endPos = new Vector2(transform.position.x, transform.position.y + endPosOffset);

        effectPos = Vector2.Lerp((Vector2)transform.position, endPos, 0.5f);

        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!razerFlg)
        {
            StartCoroutine(Razer());
        }
    }

    IEnumerator Razer()
    {
        razerFlg = true;

        float timer = 0;

        //EffectManager.Instance.Razer2PreEffect(effectPos);
        Instantiate(EffectManager.Instance.razer2PreEffect, effectPos, Quaternion.identity);

        yield return new WaitForSeconds(2.0f);

        lr.positionCount = 2;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, endPos);

        EffectManager.Instance.Razer2Effect(effectPos);

        sound.PlayOneShot(razerSound);

        while (timer < 5)
        {
            RaycastHit2D hit = Physics2D.Linecast((Vector2)transform.position, (Vector2)endPos, layer);

            if (hit)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    StartCoroutine(DamageEffectManager(hit.collider));
                    GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().PlayerDamageWhile(15.0f);
                }
                else if (hit.collider.gameObject.tag == "Tentacle")
                {
                    StartCoroutine(DamageEffectManager(hit.collider));
                    GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().PlayerDamageWhile(10.0f);
                }
            }
            timer += Time.deltaTime;

            yield return null;
        }

        lr.positionCount = 0;
        timer = 0;

        yield return new WaitForSeconds(2.0f);

        razerFlg = false;
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
