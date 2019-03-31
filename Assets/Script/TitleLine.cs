using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLine : MonoBehaviour
{
    LineRenderer lr;

    int childCount;

    List<GameObject> Nodes = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Nodes.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderLine();
    }

    void RenderLine()
    {
        lr.positionCount = childCount;

        int i;
        for (i = 0; i < Nodes.Count; i++)
        {

            lr.SetPosition(i, Nodes[i].transform.position);
        }

    }
}
