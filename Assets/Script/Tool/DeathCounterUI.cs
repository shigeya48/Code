using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounterUI : MonoBehaviour
{
    public Text enemySizeUI;

    public void EnemySize(int deathC, int startC)
    {
        enemySizeUI.text = (deathC + "/" + startC);
    }
}
