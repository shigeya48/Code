using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockFPS : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
