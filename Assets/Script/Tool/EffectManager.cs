using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    public GameObject eatPreEffect;

    public GameObject eatEffect;

    public GameObject tentacleTopEffect;

    public GameObject bulletHitEffect;

    public GameObject razerPreEffect;

    public GameObject razerEffect;

    public GameObject razer2PreEffect;

    public GameObject razer2Effect;

    public GameObject damageEffect;

    public GameObject mCoreLoadEffect;

    public GameObject coreLoadEffect;

    public GameObject playerLostEffect;
    
    public void EatPreEffect(Vector2 pos)
    {
        Instantiate(eatPreEffect, pos, Quaternion.identity);
    }

    public void EatEffect(Vector2 pos)
    {
        Instantiate(eatEffect, pos, Quaternion.identity);
    }

    public void InfectionEffect(Vector2 pos)
    {
        Instantiate(tentacleTopEffect, OffsetPos(pos, 0.5f), Quaternion.identity);
    }

    public void BulletHitEffect(Vector2 pos)
    {
        Instantiate(bulletHitEffect, OffsetPos(pos, -2), Quaternion.identity);
    }

    public void RazerPreEffect(Vector2 pos, Quaternion angle)
    {
        Instantiate(razerPreEffect, pos, angle);
    }

    public void RazerEffect(Vector2 pos, Quaternion angle)
    {
        Instantiate(razerEffect, pos, angle);
    }

    public void Razer2PreEffect(Vector2 pos)
    {
        Instantiate(razer2PreEffect, pos, Quaternion.identity);
    }

    public void Razer2Effect(Vector2 pos)
    {
        Instantiate(razer2Effect, pos, Quaternion.identity);
    }

    public void DamageEffect(Vector2 pos)
    {
        GameObject damage = Instantiate(damageEffect, OffsetPos(pos, -2.5f), Quaternion.identity);
        damage.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void MCoreLoadEffect(Vector2 pos)
    {
        Instantiate(mCoreLoadEffect, pos, Quaternion.identity);
    }

    public void CoreLoadEffect(Vector2 pos)
    {
        Instantiate(coreLoadEffect, pos, Quaternion.identity);
    }

    public void PlayerLostEffect(Vector2 pos)
    {
        Instantiate(playerLostEffect, OffsetPos(pos, -3), Quaternion.identity);
    }

    Vector3 OffsetPos(Vector2 pos, float z)
    {
        Vector3 offsetPos = new Vector3(pos.x, pos.y, z);
        return offsetPos;
    }
}
