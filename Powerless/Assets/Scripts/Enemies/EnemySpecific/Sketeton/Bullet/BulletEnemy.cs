using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    [Header("Bullet Inspector")]
    [SerializeField]
    GameObject target;

    [SerializeField]
    private Rigidbody2D bulletRB;

    [Header("Bullet Stats")]
    [SerializeField]
    private float lifeTime = 0.5f;

    public float fireRate = 0.5f;
    private float nextFireTime;

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        /* Target = GameObject.FindGameObjectWithTag("Player");                                                            //Find Target
         Vector2 moveDir = (Target.transform.position - transform.position).normalized * Speed;                          //Get position wher the player is
         BulletRB.velocity = new Vector2(moveDir.x, moveDir.y);                                                          //Destroy bullet after 2s of fly time
        */
        Destroy(gameObject, lifeTime);
    }
}
