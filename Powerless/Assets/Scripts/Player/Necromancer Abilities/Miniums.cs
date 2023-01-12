using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniums : MonoBehaviour
{
    public float LineOfSite;
    public float PersonalSpace;

    public float speed = 3f;

    public Transform Enemys;

    private Rigidbody2D _rigidbody;
    public Vector2 axisMovement;

    private void Start()
    {
        Enemys = GameObject.FindGameObjectWithTag("Enemy").transform;

        _rigidbody = GetComponent<Rigidbody2D>();


    }
    
    private void Update()
    {
        // float distanceFromEnemys = Vector2.Distance(Enemys.position, transform.position);
        chase();
    }


    private void chase()
    {
        transform.position = Vector2.MoveTowards(this.transform.position, Enemys.transform.position, speed * Time.deltaTime);
    }
}
