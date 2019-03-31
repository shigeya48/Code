using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    Vector2 bulletDirection;

    public float bulletSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)bulletDirection * bulletSpeed * Time.deltaTime;
    }

    public void BulletDirection(Vector2 direction)
    {
        bulletDirection = direction;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "EnemySquare" && col.tag != "EnemyTriangle")
        {
            if (col.tag == "Tentacle")
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowHook>().PlayerDamage(4.0f);
            }
            else if(col.tag == "Player")
            {
                col.gameObject.GetComponent<ThrowHook>().PlayerDamage(8.0f);
            }
            EffectManager.Instance.DamageEffect((Vector2)transform.position);
            Destroy(gameObject);
        }
    }
}
