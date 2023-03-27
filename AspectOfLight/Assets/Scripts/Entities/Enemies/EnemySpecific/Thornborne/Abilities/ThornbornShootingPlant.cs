using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornShootingPlant : MonoBehaviour
{
    [Header("Shooting Plant Information")]
    [SerializeField]
    private float shootingRate;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform shootingPosition;
    public Transform bulletParent;

    private IEnumerator Shoot()
    {
        while (true)
        {
            Instantiate(bullet, shootingPosition.position, Quaternion.identity, bulletParent);

            yield return new WaitForSeconds(shootingRate);
        }
    }
}
