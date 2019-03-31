using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinMap : MonoBehaviour
{
    [SerializeField]
    Image mapImage;

    Vector2 mapOffset = new Vector2(126, -86);

    Vector2 defaultOffset = new Vector2(-150, 150);

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = mapImage.GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.localPosition = (new Vector2(-transform.position.x, -transform.position.y) * 0.62f) + mapOffset + defaultOffset;
    }
}