using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "States/Skeleton/InsightFollow")]
public class InSightState : SkeletonState<SkeletonController>
{
    private Transform player;
    
    public float LineOfSite;

    private Rigidbody2D _rigidbody;
    private float speed = 3f;
    //  private bool InsightFollow = false;

    GameObject skeletonComponent;

    public override void Init(SkeletonController parent)
    {
       // InsightFollow = false;

        if (_rigidbody == null)
            _rigidbody = parent.GetComponent<Rigidbody2D>();

        if (player == null)
            player = parent.GetComponent<Transform>();

        skeletonComponent = parent.transform.gameObject;
        Debug.Log(player);
        player = GameObject.FindGameObjectWithTag("Player").transform;

        base.Init(parent);
    }
    public override void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, skeletonComponent.transform.position);
        Debug.Log("ismoving");
        skeletonComponent.transform.position = Vector2.MoveTowards(this.skeletonComponent.transform.position, player.transform.position, speed * Time.deltaTime);
    }
    public override void FixedUpdate() { }
    public override void ChangeState() { }
    public override void Exit() { }

}