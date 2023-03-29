using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [Header("Information")]
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    [Range(0.0f, 1f)]
    private float interested;

    void LateUpdate()
    {
        if (parent == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            parent.transform.position,
            interested
        );
    }
}
