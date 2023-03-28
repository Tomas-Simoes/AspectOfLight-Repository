using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Entity
{
    [Header("Pathing")]
    private Seeker seeker;
    public Path path;
    public int currentWaypoint = 0;

    [Header("Informations")]
    public GameObject playerReference;

    [Space(5)]
    public bool goTowardsOtherPatrolPoint; //TODO change this to EnemyPatrolState
    public GameObject[] patrolPoints;

    [Header("Enemy Boss Information")]
    public bool isFightStarted = false;

    public override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.2f); //TODO mayyybe change this to a Coroutine

        currentHealth = entityData.MAX_HEALTH;
    }

    public override void Update()
    {
        base.Update();

        UpdateWaypoints();
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            this.path = path;
            currentWaypoint = 0;
        }
    }

    private void UpdateWaypoints()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
            return;

        if (Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]) < 0.1f)
        {
            currentWaypoint++;
        }
    }
}
