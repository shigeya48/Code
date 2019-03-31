using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysDeathController : MonoBehaviour
{
    GameObject[] enemys;

    public GameObject deathCounterUIPre;

    bool[] deathFlg;

    bool allDeathFlg = false;

    int deathCounter = 0;

    int startEnemyCount = 0;

    // Use this for initialization
    void Start()
    {
        enemys = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            enemys[i] = transform.GetChild(i).gameObject;
        }

        deathFlg = new bool[enemys.Length];

        startEnemyCount = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemys.Length; i++)
        {
            if (enemys[i] != null)
            {
                if (enemys[i].tag == "EnemySquare")
                {
                    if (enemys[i].GetComponent<EnemySquare>().DeathFlg() && !deathFlg[i])
                    {
                        deathFlg[i] = true;
                        deathCounter++;
                        GameObject deathCounterUI = Instantiate(deathCounterUIPre, enemys[i].transform.position + new Vector3(0, 3, 0), Quaternion.identity);

                        deathCounterUI.GetComponent<DeathCounterUI>().EnemySize(deathCounter, startEnemyCount);
                    }
                }
                else if (enemys[i].tag == "EnemyTriangle")
                {
                    if (enemys[i].GetComponent<EnemyTriangle>().DeathFlg() && !deathFlg[i])
                    {
                        deathFlg[i] = true;
                        deathCounter++;

                        GameObject deathCounterUI = Instantiate(deathCounterUIPre, enemys[i].transform.position + new Vector3(0, 3, 0), Quaternion.identity);

                        deathCounterUI.GetComponent<DeathCounterUI>().EnemySize(deathCounter, startEnemyCount);
                    }
                }
            }
        }

        if (deathCounter >= enemys.Length)
        {
            allDeathFlg = true;
        }

    }

    public bool AllDeath()
    {
        return allDeathFlg;
    }
}
