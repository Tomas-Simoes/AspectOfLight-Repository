using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicYSort : MonoBehaviour
{
    private float ySortingOffset;

    [SerializeField]
    private Transform sortOffset;

    void Start()
    {
        ySortingOffset = sortOffset.position.y;
    }

    void Update()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = transform.GetSortingLayer(ySortingOffset);
        Debug.Log(transform.GetSortingLayer(ySortingOffset));
    }
}
