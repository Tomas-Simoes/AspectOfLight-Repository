using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniums : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject closestEnemy;

    public Transform Enemys;
 

    public float speed = 3f;

    private Rigidbody2D _rigidbody;
    public Vector2 axisMovement;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag ("enemy");

        _rigidbody = GetComponent<Rigidbody2D>();

    }
    
    private void Update()
    {
        FindClosestEnemy();
    }

    void FindClosestEnemy()
    {
     float distance = 100f; // Any large number will do as long as it's further than any enemy could be
     foreach (GameObject e in enemies) {
         float tempdist = Vector3.Distance (transform.position,e.transform.position);
         if (tempdist < distance)
            {
             closestEnemy = e;
             distance = tempdist;
            }
            transform.position = Vector2.MoveTowards(this.transform.position, Enemys.transform.position, speed * Time.deltaTime);
        }
    }
}
