using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornborneShootingPlant : MonoBehaviour
{
    [Header("Shooting Plant Information")]
    [SerializeField]
    private float shootingRate;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform shootingPosition;

    public GameObject plantTarget;
    public Transform bulletParent;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            GameObject _bullet = Instantiate(
                bullet,
                shootingPosition.position,
                Quaternion.identity,
                bulletParent
            );

            _bullet.GetComponent<Bullet>().targetPosition = plantTarget.transform.position;

            yield return new WaitForSeconds(shootingRate);
        }
    }
}
