using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimPosition : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private float offset;

    void Update()
    {
        Aiming();
    }

    private void Aiming()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 playerToMouseDir =
            Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        playerToMouseDir.z = 0;

        transform.position = player.position + (offset * playerToMouseDir.normalized);
        Vector3 AimLocalScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            AimLocalScale.y = -1f;
        }
        else
        {
            AimLocalScale.y = 1f;
        }

        transform.localScale = AimLocalScale;
    }
}
