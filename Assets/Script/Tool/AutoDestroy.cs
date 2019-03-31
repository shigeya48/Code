using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    float destroyTimer;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, destroyTimer);
	}
	
}
