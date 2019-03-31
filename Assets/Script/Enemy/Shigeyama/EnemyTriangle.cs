using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriangle : MonoBehaviour
{
    GameObject player;

    Rigidbody2D rd2;

    int moveCounter = 0;

    float timer = 2.0f;
    float timeInterval = 2.0f;

    float playerDistance = 40.0f;

    public float moveSpeed;

    bool attackFlg = false;

    bool deathFlg = false;

    bool deathControllerFlg = false;

    bool damageFlg = false;

    public LayerMask Wall;

    Animator anim;

    public AudioClip razerSound;

    AudioSource source;

    Vector2[] moveDirections =
    {
        new Vector2( 0.0f, -1.0f),
        new Vector2( 0.0f,  1.0f)
    };

    GameObject[] triangleTop = new GameObject[3];

    LineRenderer[] lr = new LineRenderer[3];

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rd2 = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        triangleTop[0] = transform.GetChild(0).gameObject;
        triangleTop[1] = transform.GetChild(1).gameObject;
        triangleTop[2] = transform.GetChild(2).gameObject;

        lr[0] = triangleTop[0].GetComponent<LineRenderer>();
        lr[1] = triangleTop[1].GetComponent<LineRenderer>();
        lr[2] = triangleTop[2].GetComponent<LineRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeInterval)
        {
            timer = 0;
            EnemyMove();
        }

        if (moveCounter >= moveDirections.Length)
        {
            moveCounter = 0;
        }

        if (Vector2.Distance(player.transform.position, transform.position) < playerDistance && !deathFlg)
        {
            Vector2 playerDirection = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + playerDirection * 2, playerDirection);

            if (hit.collider.gameObject.tag == "Player")
            {
                if (!attackFlg)
                {
                    attackFlg = true;
                    // 攻撃
                    StartCoroutine(EnemyAttack());
                }
            }
        }
    }

    void EnemyMove()
    {
        rd2.velocity = Vector3.zero;

        if (moveCounter < moveDirections.Length)
        {
            rd2.velocity += moveDirections[moveCounter] * moveSpeed * Time.deltaTime;
        }
        else
        {
            timer = timeInterval;
        }
        moveChecker();

    }

    IEnumerator EnemyAttack()
    {
        anim.SetTrigger("RockOn");

        yield return new WaitForSeconds(1.0f);

        anim.SetBool("Attack", true);

        yield return new WaitForSeconds(1.0f);

        int j = 0;

        transform.GetChild(3).GetComponent<PolygonCollider2D>().enabled = true;

        float timer = 0;
        float timeInterval = 3.0f;

        source.PlayOneShot(razerSound);

        while (timer < timeInterval)
        {
            timer += Time.deltaTime;
            for (int i = 0; i < triangleTop.Length; i++)
            {
                lr[i].positionCount = 3;
                lr[i].SetPosition(0, transform.position);
                lr[i].SetPosition(1, triangleTop[i].transform.position);
                if (i + 1 >= 3)
                {
                    j = 0;
                }
                lr[i].SetPosition(2, triangleTop[j].transform.position);
                j++;
            }
            yield return null;
        }

        lr[0].positionCount = 0;
        lr[1].positionCount = 0;
        lr[2].positionCount = 0;
        transform.GetChild(3).GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        anim.SetBool("Attack", false);

        yield return new WaitForSeconds(2.0f);

        attackFlg = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            timer = timeInterval;
            moveChecker();
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player.GetComponent<ThrowHook>().PlayerDamageWhile(5.0f);
        }
        if (col.gameObject.tag == "Tentacle")
        {
            StartCoroutine(DamageEffectManager(col));
            player.GetComponent<ThrowHook>().PlayerDamageWhile(1.0f);
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

    void moveChecker()
    {
        if (moveCounter >= moveDirections.Length)
        {
            moveCounter = 0;
        }
        else
        {
            moveCounter++;
        }
    }

    public void DeathTrianglePreparation()
    {
        deathFlg = true;
        rd2.simulated = false;
        moveSpeed = 0;
    }

    public void DeathTriangle()
    {
        deathControllerFlg = true;

        Destroy(gameObject, 0.1f);
    }

    public bool DeathFlg()
    {
        return deathControllerFlg;
    }
}
