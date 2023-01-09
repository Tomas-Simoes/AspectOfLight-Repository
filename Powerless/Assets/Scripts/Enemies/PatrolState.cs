using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Skeleton/Patrol")]
public class PatrolState : EnemyState<SkeletonController>
{
    int currentPatrolIndex;

    GameObject currentPatrolPoint;
    GameObject skeletonComponent;
    
    EnemyInfo enemyInfo;

    public override void Init(SkeletonController parent)
    {

        if (enemyInfo == null)
            enemyInfo = parent.GetComponent<EnemyInfo>();


        currentPatrolIndex = 0;
        currentPatrolPoint = enemyInfo.PatrolPoint[currentPatrolIndex];

        skeletonComponent = parent.gameObject;

        base.Init(parent);
    }
    public override void Update()
    {
        GameObject waypoint = enemyInfo.PatrolPoint[currentPatrolIndex];
        if (Vector3.Distance(skeletonComponent.transform.position, waypoint.transform.position) < 0.01f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % enemyInfo.PatrolPoint.Length;
        }
        else
        {
            skeletonComponent.transform.position = Vector3.MoveTowards(
                skeletonComponent.transform.position,
                waypoint.transform.position,
                enemyInfo.speed * Time.deltaTime);
        }
    }
    public override void FixedUpdate() { }
    public override void ChangeState() { }
    public override void Exit() { }
}
