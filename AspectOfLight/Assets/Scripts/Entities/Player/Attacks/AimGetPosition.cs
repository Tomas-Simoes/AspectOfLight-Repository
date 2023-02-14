using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimGetPosition : MonoBehaviour
{
    [SerializeField]
    public Camera Cam;

    void Update()
    {
        Aiming();
    }

    private void Aiming()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = Cam.ScreenToWorldPoint(mousePos);
        Debug.Log(mousePos.x - transform.position.x);
        Debug.Log(mousePos.y - transform.position.y);
        Vector2 direc = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        transform.right = direc;
    }
}
