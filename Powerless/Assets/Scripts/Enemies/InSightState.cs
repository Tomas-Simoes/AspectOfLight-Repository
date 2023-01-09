using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "States/Skeleton/InsightFollow")]
public class InSightState : EnemyState<SkeletonController>
{   
    public float LineOfSite;

    EnemyInfo enemyinfo;

    Rigidbody2D _rigidbody;

    //  private bool InsightFollow = false;

    GameObject skeletonComponent;

    public override void Init(SkeletonController parent)
    {
       // InsightFollow = false;

        if (_rigidbody == null)
            _rigidbody = parent.GetComponent<Rigidbody2D>();

        if (enemyinfo == null)
            enemyinfo = parent.GetComponent<EnemyInfo>();

        skeletonComponent = parent.transform.gameObject;
        Debug.Log(enemyinfo.target);
        enemyinfo.target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Init(parent);
    }
    public override void Update()
    {
        float distanceFromPlayer = Vector2.Distance(enemyinfo.target.position, skeletonComponent.transform.position);
        Debug.Log("ismoving");
        skeletonComponent.transform.position = Vector2.MoveTowards(this.skeletonComponent.transform.position, enemyinfo.target.transform.position, enemyinfo.speed * Time.deltaTime);
    }
    public override void FixedUpdate() { }
    public override void ChangeState() { }
    public override void Exit() { }

}