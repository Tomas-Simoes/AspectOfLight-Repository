using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornbornePlant : Entity
{
    public ThornbornePlant_ShootingState shootingState { get; private set; }

    [Header("Shooting Plant Information")]
    public float shootingRate;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform shootingPosition;

    public GameObject plantTarget;
    public GameObject bulletParent;

    public override void Start()
    {
        base.Start();

        shootingState = new ThornbornePlant_ShootingState(this, stateMachine, "shooting", this);

        stateMachine.Init(shootingState);
    }

    public void StartShooting()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            GameObject _bullet = CreatePrefab(bullet, shootingPosition.position, bulletParent);

            _bullet.GetComponent<Bullet>().targetPosition = plantTarget.transform.position;

            yield return new WaitForSeconds(shootingRate);
        }
    }
}
