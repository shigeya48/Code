using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : MonoBehaviour
{
    
    GameObject player;

    Rigidbody2D rd2;

    public GameObject bulletPre;

    int moveCounter = 0;

    int bulletCounter = 4;

    float timer = 2.0f;
    float timeInterval = 2.0f;

    float playerDistance = 40.0f;

    public float moveSpeed;

    bool attackFlg = false;

    bool deathFlg = false;

    bool deathControllerFlg = false;

    Animator anim;

    public AudioClip shotSound;

    AudioSource source;

    Vector2[] moveDirections =
    {
        new Vector2( 0.5f, -1.0f),
        new Vector2(-0.5f, -1.0f),
        new Vector2(-0.5f,  1.0f),
        new Vector2( 0.5f,  1.0f)
    };

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rd2 = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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
        anim.SetTrigger("LockOn");

        yield return new WaitForSeconds(1.5f);

        anim.SetBool("Attack", true);
        for(int i = 0; i < bulletCounter; i++)
        {
            if (deathFlg)
            {
                break;
            }

            GameObject bullet = Instantiate(bulletPre, transform.position, Quaternion.identity);
            Vector2 bulletDirection = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            bullet.GetComponent<EnemyBulletScript>().BulletDirection(bulletDirection);

            source.PlayOneShot(shotSound);

            yield return new WaitForSeconds(0.4f);
        }

        anim.SetBool("Attack", false);
        attackFlg = false;

        yield return null;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            timer = timeInterval;
            moveChecker();
        }
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

    public void DeathSquarePreparation()
    {
        deathFlg = true;
        rd2.simulated = false;
        moveSpeed = 0;
    }

    public void DeathSquare()
    {
        deathControllerFlg = true;
        Destroy(gameObject, 0.1f);
    }

    public bool DeathFlg()
    {
        return deathControllerFlg;
    }
}