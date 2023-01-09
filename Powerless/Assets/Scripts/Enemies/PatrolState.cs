using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Skeleton/Patrol")]
public class PatrolState : SkeletonState<SkeletonController>
{

    public Transform[] patrolPoints;

    int currentPatrolIndex;
    private Transform player;


    Transform currentPatrolPoint;
    GameObject PatrolPoint;
    GameObject skeletonComponent;

    private float speed = 3;                                      

    public override void Init(SkeletonController parent)
    {
        currentPatrolIndex = 0;
        currentPatrolPoint = patrolPoints[currentPatrolIndex];

        base.Init(parent);
    }
    public override void Update()
    {
        Transform wp = patrolPoints[currentPatrolIndex];
        if (Vector3.Distance(skeletonComponent.transform.position, wp.position) < 0.01f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
        else
        {
            skeletonComponent.transform.position = Vector3.MoveTowards(
                skeletonComponent.transform.position,
                wp.position,
                speed * Time.deltaTime);
        }
    }
    public override void FixedUpdate() { }
    public override void ChangeState() { }
    public override void Exit() { }
}
